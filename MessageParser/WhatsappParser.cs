using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Text;
using Helper;

namespace MessageParser
{
    public class WhatsappParser
    {
        private List<string> _lines;
        private ImageMatcher _imageMatcher;
        private int _index;
        private Tuple<DateTime, int> _lastcnt;
		
        // TODO extend pattern with Nickname and Message
        private static readonly string DATEPATTERN = "[0-3][0-9]/[0-1][0-9]/[0-9]{4},\\ [0-2][0-9]:[0-5][0-9]";
        private static readonly string PATTERN = DATEPATTERN + "\\ -\\ ";
        private static readonly string FILE_ATTACHED = "(file attached)";
        private static readonly string MEDIA_OMITTED = "<Media omitted>";

        public WhatsappParser(string messagePath, ImageMatcher imageMatcher)
        {
            _lines = File.ReadAllLines(messagePath).ToList();
            _index = 0;

            _imageMatcher = imageMatcher;
            _lastcnt = new Tuple<DateTime,int>(DateTime.MinValue, -1);
        }

        public IMessage NextMessage()
        {
            if (_index == _lines.Count)
            {
                return null;
            }
			
            var line = _lines[_index];
            if (!IsHeader(line))
            {
                throw new ArgumentException($"Invalid header line: '{line}'");
            }

            _index++;

            var dateStr = Regex.Match(line, DATEPATTERN).Value;
            var date = DateTime.ParseExact(dateStr, "dd/MM/yyyy, HH:mm", CultureInfo.InvariantCulture);
            // TODO use regex
            int senderEnd = line.IndexOf(':', dateStr.Length);

            // spezial message, e.g. encryption information
            if (senderEnd == -1)
            {
                Console.WriteLine($"No sender found, skipping line '{line}'");
                return NextMessage();
            }

            var sender = line.Substring(dateStr.Length + 3, senderEnd - dateStr.Length - 3);
            var contentStr = line.Substring(senderEnd + 2);

            if (contentStr.EndsWith(FILE_ATTACHED))
            {
                var fileName = contentStr.Substring(0, contentStr.Length - FILE_ATTACHED.Length - 1);
                var extension = fileName.Substring(fileName.Length - 3);

                switch (extension)
                {
                    case "jpg":
                        var subscription = ParseNextLines().Trim();
                        return new ImageMessage(date, sender, fileName, subscription);
                    default:
                        subscription = ParseNextLines().Trim();
                        return new MediaMessage(date, sender, fileName, subscription);
                }
            }
            else if (contentStr.Equals(MEDIA_OMITTED))
            {
                var entry = _imageMatcher.Pick(date, GetCnt(date));
                if (entry.IsImage && entry.Filematches.Count() > 0)
                {
                    return new MediaOmittedMessage(entry, sender);
                }
                else
                {
                    return NextMessage();
                }
            }
            else
            {
                contentStr = contentStr + ParseNextLines();
                contentStr = contentStr.Trim();
                return new TextMessage(date, sender, contentStr);
            }
        }

        private int GetCnt(DateTime tp)
        {
            if (_lastcnt.Item2 == -1)
            {
                _lastcnt = new Tuple<DateTime, int>(tp, 0);
            }
            else
            {
                if (_lastcnt.Item1.Equals(tp))
                {
                    _lastcnt = new Tuple<DateTime, int>(_lastcnt.Item1, _lastcnt.Item2 + 1);
                }
                else
                {
                    _lastcnt = new Tuple<DateTime, int>(tp, 0);
                }
            }

            return _lastcnt.Item2;
        }

        private bool IsHeader(string str)
        {
            var match = Regex.Match(str, PATTERN);
            return match.Success && match.Index == 0;
        }

        private string ParseNextLines()
        {
            var sb = new StringBuilder();
            while (_index < _lines.Count && !IsHeader(_lines[_index]))
            {
                sb.AppendLine();
                sb.Append(_lines[_index]);
                _index++;
            }

            return sb.ToString();
        }
    }
}

