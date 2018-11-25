using System;

namespace Helper
{
    public class Latex
    {
        public static string EncodeLatex(string str)
        {
            if (str == null)
            {
                return string.Empty;
            }

            str = str.Replace("\\", "\\\\");
            str = str.Replace("_", "\\_");
            str = str.Replace("{", "\\{");
            str = str.Replace("}", "\\}");
            str = str.Replace("\"", "''");

            str = str.Replace("\n", "\\\\");
            str = str.Replace("&", "\\&");
            str = str.Replace("#", "\\#");
            str = str.Replace("%", "\\%");
            str = str.Replace("$", "\\$");
            str = str.Replace("€", "\\euro ");
            str = str.Replace("„", "\\glqq ");
            str = str.Replace("“", "\\grqq ");
            str = str.Replace("[", "");
            str = str.Replace("]", "");
            str = str.Replace("^", "$\\hat{}$");
            str = str.Replace("<", "$<$");
            str = str.Replace(">", "$>$");
            str = str.Replace("’", "'");
            str = str.Replace("…", "...");
            str = str.Replace("°", "${}^\\circ$");

            str = str.Replace("ß", "\\ss{}");
            str = str.Replace("Ä", "\\\"A");
            str = str.Replace("Ö", "\\\"O");
            str = str.Replace("Ü", "\\\"U");
            str = str.Replace("ä", "\\\"a");
            str = str.Replace("ö", "\\\"o");
            str = str.Replace("ü", "\\\"u");

            // àáâ
            str = str.Replace("à", "\\`{a}");
            str = str.Replace("á", "\\'{a}");
            str = str.Replace("â", "\\^{a}");
            str = str.Replace("À", "\\`{A}");
            str = str.Replace("Á", "\\'{A}");
            str = str.Replace("Â", "\\^{A}");

            // èéê
            str = str.Replace("è", "\\`{e}");
            str = str.Replace("é", "\\'{e}");
            str = str.Replace("ê", "\\^{e}");
            str = str.Replace("È", "\\`{E}");
            str = str.Replace("É", "\\'{E}");
            str = str.Replace("Ê", "\\^{E}");

            // ìíî
            str = str.Replace("ì", "\\`{i}");
            str = str.Replace("í", "\\'{i}");
            str = str.Replace("î", "\\^{i}");
            str = str.Replace("Ì", "\\`{I}");
            str = str.Replace("Í", "\\'{I}");
            str = str.Replace("Î", "\\^{I}");

            // òóô
            str = str.Replace("ò", "\\`{o}");
            str = str.Replace("ó", "\\'{o}");
            str = str.Replace("ô", "\\^{o}");
            str = str.Replace("Ò", "\\`{O}");
            str = str.Replace("Ó", "\\'{O}");
            str = str.Replace("Ô", "\\^{O}");

            // ùúû
            str = str.Replace("ù", "\\`{u}");
            str = str.Replace("ú", "\\'{u}");
            str = str.Replace("û", "\\^{u}");
            str = str.Replace("Ù", "\\`{U}");
            str = str.Replace("Ú", "\\'{U}");
            str = str.Replace("Û", "\\^{U}");



            /*// causes strange character 0xC2 0xA0 --> " "
			str = ReplaceHelper.ReplaceString (str, new byte[]{ 0xC2, 0xA0 }, " "); 

			// 0xC2 0x84 --> ,,
			str = ReplaceHelper.ReplaceString (str, new byte[]{ 0xC2, 0x84 }, "\\glqq ");

			// 0xC2 0x93 --> ''
			str = ReplaceHelper.ReplaceString (str, new byte[]{ 0xC2, 0x93 }, "\\grqq ");

			// 0xC2 0x92 --> '
			str = ReplaceHelper.ReplaceString (str, new byte[]{ 0xC2, 0x92 }, "'");

			// 0xC2 0x85 --> nothing
			str = ReplaceHelper.ReplaceString (str, new byte[]{ 0xC2, 0x85 }, "");*/

            return str;
        }

        public static string ReplaceURL(string str)
        {
            bool found = true;
            int startIndex = 0;
            while (found)
            {
                var result = FirstString(str, new string[] { "http://", "https://" }, startIndex);

                if (result.Item1 != -1)
                {
                    int whiteIndex = FirstString(str, new string[]{ " ", "\n", @"\\" }, result.Item1).Item1;

                    string left = str.Substring(0, result.Item1);
                    string httpString;
                    string right;
                    if (whiteIndex == -1)
                    {
                        httpString = str.Substring(result.Item1);
                        right = string.Empty;
                    }
                    else
                    {
                        httpString = str.Substring(result.Item1, whiteIndex - result.Item1);
                        right = str.Substring(whiteIndex);
                    }

                    var replaceStr = @"\url{" + httpString + "}";
                    str = left + replaceStr + right;
                    startIndex = result.Item1 + replaceStr.Length;
                }
                else
                {
                    found = false;
                }
            }

            return str;
        }

        private static Tuple<int, string> FirstString(string str, string[] needles, int startIndex)
        {
            string needle = null;
            int index = -1;
            foreach (var x in needles)
            {
                int i = str.IndexOf(x, startIndex);
                if (i != -1 && (i < index || needle == null))
                {
                    index = i;
                    needle = x;
                }
            }

            return new Tuple<int, string>(index, needle);
        }
    }
}

