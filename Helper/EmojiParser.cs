using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Helper
{
    public class EmojiParser
    {
        public List<string> EmojiList
        {
            get;
            set;
        }

        public Func<string, string> FormatFunction
        {
            get;
            set;
        }

        public static StreamWriter Debug
        {
            get;
            set;
        }

        private int _tokenMax = 0;

        private readonly char SEPERATOR = '_';

        /// <summary>
        /// Initializes a new instance of the <see cref="EmojiParser.Parser"/> class.
        /// </summary>
        /// <param name="emojiList">List of known emojis</param>
        /// <param name="formatFunction">FormatFunction, input hex values, output include graphics command</param>
        public EmojiParser(List<string> emojiList, Func<string,string> formatFunction)
        {
            EmojiList = emojiList;
            FormatFunction = formatFunction;

            foreach (var x in EmojiList)
            {
                _tokenMax = Math.Max(_tokenMax, x.Split(SEPERATOR).Length);
            }

            _tokenMax++;
        }

        public string ReplaceEmojis(string str)
        {
            var sb = new StringBuilder();
            var utf32 = Encoding.UTF32.GetBytes(str);
            int index = 0;
            while (index < utf32.Length)
            {
                index = ParseQuadruple(utf32, index, sb);
            }

            return sb.ToString();
        }

        private int ParseQuadruple(byte[] utf32, int index, StringBuilder sb, string last = null, int cnt = 0)
        {
            if (cnt == _tokenMax)
            {
                return -1;
            }

            if (index == utf32.Length)
            {
                return -1;
            }
			
            var quad = new byte[4];
            Array.Copy(utf32, index, quad, 0, 4);
            var strHex = Convert(quad);
			
            var suggestion = strHex;
            if (last != null)
            {
                suggestion = last + SEPERATOR + suggestion;
            }

            int result = ParseQuadruple(utf32, index + 4, sb, suggestion, cnt + 1);
            if (result == -1)
            {
                if (EmojiList.Contains(suggestion))
                {
                    sb.Append(FormatFunction(suggestion));
                    return index + 4;
                }
                else
                {
                    if (cnt == 0)
                    {
                        var replacement = Encoding.UTF32.GetString(utf32, index, 4);

                        // See if it is an SoftBank encoded character
                        var alternative = SoftBankConverter.GetNewUnicode(suggestion);
                        if (alternative != null)
                        {
                            if (EmojiList.Contains(alternative))
                            {
                                replacement = FormatFunction(alternative);
                            }
                        }

                        
                        sb.Append(replacement);
                        if (Debug != null)
                        {
                            int dbgVal = BitConverter.ToInt32(quad, 0);
                            Debug.WriteLine($"{dbgVal} {alternative}");
                        }
                        return index + 4;
                    }
                }

                return -1;
            }
            else
            {
                return result;
            }
        }

        private string Convert(byte[] arr)
        {
            arr = Invert(arr);
            var result = BitConverter.ToString(arr).Replace("-", "").TrimStart('0').ToLower();

            // if result has less than 4 characters add leading zeros
            while (result.Length < 4)
            {
                result = '0' + result;
            }

            return result;
        }

        private byte[] Invert(byte[] arr)
        {
            var len = arr.Length;
            var result = new byte[len];
            for (int i = 0; i < len; i++)
            {
                result[i] = arr[len - 1 - i];
            }

            return result;
        }
    }
}