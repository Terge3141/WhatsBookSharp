using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Xml.Serialization;

namespace Helper
{
    public class ImageMatcher
    {
        private List<MatchEntry> _matchList;

        public List<FileEntry> FileList
        {
            get;
            set;
        }

        public bool SearchMode
        {
            get;
            set;
        }

        public ImageMatcher()
        {
            _matchList = new List<MatchEntry>();
        }

        public MatchEntry Pick(DateTime timepoint, int cnt = 0)
        {
            var query = _matchList.Where(x => (x.Timepoint.Equals(timepoint) && x.Cnt == cnt));
            int qcnt = query.Count();
            if (qcnt != 1)
            {
                if (SearchMode)
                {
                    var matches = FileList.Where(x => DateEqual(x.Timepoint, timepoint)).ToList();
                    var matchEntry = new MatchEntry(timepoint, matches, cnt);
                    _matchList.Add(matchEntry);
                    return matchEntry;
                }
                else
                {
                    throw new ArgumentException($"Invalid number of entries found ({qcnt}), 1 expected");
                }
            }

            return query.First();
        }

        public List<MatchEntry> GetMatchList()
        {
            return _matchList;
        }

        public void Save(string path)
        {
            File.WriteAllText(path, SaveToString());
        }

        public string SaveToString()
        {
            return Serializer.SerializeToXml(_matchList);
        }

        public void LoadMatches(string path)
        {
            LoadFromString(File.ReadAllText(path));
        }

        public void LoadFromString(string xmlstr)
        {
            _matchList = Serializer.DeserializeFromXml<List<MatchEntry>>(xmlstr);
        }

        private bool DateEqual(DateTime dt1, DateTime dt2)
        {
            return dt1.Year == dt2.Year && dt1.Month == dt2.Month && dt1.Day == dt2.Day;
        }

        public void LoadFiles(string dir)
        {
            var files = Directory.EnumerateFiles(dir, "*.jp*", SearchOption.AllDirectories);
            FileList = new List<FileEntry>();

            foreach (var file in files)
            {
                var entry = new FileEntry(file, dir);

                if (FileList.Where(x => x.Filename.Equals(entry.Filename)).Count() > 0)
                {
                    Console.WriteLine($"Skipping {entry.Relpath}");
                }
                else
                {
                    FileList.Add(entry);
                }
            }
        }

        public class FileEntry
        {
            // for serialization
            public FileEntry()
            {
            }

            public FileEntry(string fullpath, string prefix)
            {
                if (!fullpath.StartsWith(prefix))
                {
                    throw new ArgumentException($"Fullpath '{fullpath}' does not start with '{prefix}'");
                }

                Relpath = fullpath.Substring(prefix.Length + 1);
                Filename = Path.GetFileName(fullpath);
                Timepoint = GetTimepoint(Filename);
            }

            public DateTime Timepoint
            {
                get;
                set;
            }

            public string Filename
            {
                get;
                set;
            }

            public string Relpath
            {
                get;
                set;
            }

            private DateTime GetTimepoint(string filename)
            {
                var pattern = "IMG-[0-9]{8}-WA[0-9]{4}.jp";
                var match = Regex.Match(filename, pattern);

                if (!match.Success || match.Index != 0)
                {
                    throw new ArgumentException($"Invalid filename '{filename}'");
                }

                var dateStr = filename.Substring(4, 8);
                return DateTime.ParseExact(dateStr, "yyyyMMdd", CultureInfo.InvariantCulture);
            }
        }

        public class MatchEntry
        {
            // for serialization
            public MatchEntry()
            {
            }

            public MatchEntry(DateTime timepoint, List<FileEntry> filematches, int cnt)
            {
                Timepoint = timepoint;
                Filematches = filematches;
                Cnt = cnt;
                IsImage = true;
            }

            public DateTime Timepoint
            {
                get;
                set;
            }

            public bool IsImage
            {
                get;
                set;
            }

            public List<FileEntry> Filematches
            {
                get;
                set;
            }

            public int Cnt
            {
                get;
                set;
            }
        }
    }
}

