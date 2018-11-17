using System;
using System.IO;
using System.Collections.Generic;
using Helper;
using MessageParser;
using System.Text;
using System.Linq;

namespace WhatsBookSharp
{
    public class BookCreator
    {
        private EmojiParser _emojis;
        private string _header;
        private string _footer;

        private static string[] _months = new string[]
        {
            "Januar",
            "Februar",
            "März",
            "April",
            "Mai",
            "Juni",
            "Juli",
            "August",
            "September",
            "Oktober",
            "November",
            "Dezember"
        };

        /// <summary>
        /// Gets or sets the top level input directory. It typically contains the subdirectories chat and config
        /// </summary>
        /// <value>The input dir.</value>
        public string InputDir
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the output dir. This is the directory where the tex file and other output files are written
        /// </summary>
        /// <value>The output dir.</value>
        public string OutputDir
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the chat dir. It is the directory where the chat txt file and the images are stored.
        /// These files can be obtained by exporting the chat in the Whatsapp app
        /// By default the directory is set to InputDir/Chat
        /// </summary>
        /// <value>The chat dir.</value>
        public string ChatDir
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the config dir. In this directory all configuration files are, e.g. the chatname.match.xml file
        /// </summary>
        /// <value>The config dir.</value>
        public string ConfigDir
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the image dir. It contains all images for the chat.
        /// By default it is set to ChatDir
        /// </summary>
        /// <value>The image dir.</value>
        public string ImageDir
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the image pool. It should contain all whatsapp images.
        /// This directory is used if there "<Media omitted>" lines in the chat file.
        /// and if no chatname.match.xml file is available.
        /// It is set to null by default.
        /// </summary>
        /// <value>The image pool.</value>
        public string ImagePoolDir
        {
            get;
            set;
        }

        public BookCreator(string inputDir, string outputDir, string emojiDir)
        {
            var emojiList = ReadEmojiList(emojiDir);
            var format = "\\includegraphics[scale=0.15]{{" + emojiDir + "/{0}.png}}";
            _emojis = new EmojiParser(emojiList, format);

            _header = File.ReadAllText("header.tex.tmpl");
            _footer = File.ReadAllText("footer.tex.tmpl");

            InputDir = inputDir;
            OutputDir = outputDir;
            ChatDir = Path.Combine(InputDir, "chat");
            ConfigDir = Path.Combine(InputDir, "config");
            ImageDir = ChatDir;
            ImagePoolDir = null;
        }

        public void WriteTex()
        {
            var txtFiles = Directory.EnumerateFiles(ChatDir, "*.txt");
            if (txtFiles.Count() != 1)
            {
                throw new ArgumentException("Invalid number of .txt-files found: " + txtFiles.Count());
            }

            var txtInputPath = txtFiles.First();
            Console.WriteLine($"Using {txtInputPath} as input");

            var namePrefix = Path.GetFileName(txtInputPath);
            namePrefix = namePrefix.Substring(0, namePrefix.Length - 4);
            var texOutputPath = Path.Combine(OutputDir, namePrefix + ".tex");

            var matchInputPath = Path.Combine(ConfigDir, namePrefix + ".match.xml");
            var im = new ImageMatcher();
            if (File.Exists(matchInputPath))
            {
                Console.WriteLine($"Loading matches '{matchInputPath}'");
                im.LoadMatches(matchInputPath);
                im.SearchMode = false;
            }
            else
            {
                if (ImagePoolDir == null)
                {
                    im.SearchMode = false;
                }
                else
                {
                    Console.WriteLine($"Loading pool images from '{ImagePoolDir}'");
                    im.LoadFiles(ImagePoolDir);
                    im.SearchMode = true;
                }
            }
			
            var parser = new WhatsappParser(txtInputPath, im);

            var sb = new StringBuilder();
            sb.AppendLine(_header);

            IMessage msg;
            DateTime last = DateTime.MinValue;
            while ((msg = parser.NextMessage()) != null)
            {
                if (TimeDiffer(last, msg.Timepoint))
                {
                    sb.AppendLine(@"\begin{center}" + GetDateString(msg.Timepoint) + @"\end{center}");
                }

                last = msg.Timepoint;

                if (msg is TextMessage)
                {
                    AppendTextMessage(msg as TextMessage, sb);
                }
                else if (msg is ImageMessage)
                {
                    AppendImageMessage(msg as ImageMessage, sb);
                }
                else if (msg is MediaOmittedMessage)
                {
                    AppendMediaOmittedMessage(msg as MediaOmittedMessage, sb);
                }
                else if (msg is MediaMessage)
                {
                    AppendMediaMessage(msg as MediaMessage, sb);
                }
            }

            sb.AppendLine(_footer);

            File.WriteAllText(texOutputPath, sb.ToString());
            Console.WriteLine($"Written to '{texOutputPath}'");
            im.Save(Path.Combine(OutputDir, namePrefix + ".match.xml"));
        }

        private static bool TimeDiffer(DateTime date1, DateTime date2)
        {
            return date1.Year != date2.Year || date1.Month != date2.Month || date1.Day != date2.Day;
        }

        private static string GetDateString(DateTime date)
        {
            var dayName = "UNKNOWN";
            switch (date.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    dayName = "Montag";
                    break;
                case DayOfWeek.Tuesday:
                    dayName = "Dienstag";
                    break;
                case DayOfWeek.Wednesday:
                    dayName = "Mittwoch";
                    break;
                case DayOfWeek.Thursday:
                    dayName = "Donnerstag";
                    break;
                case DayOfWeek.Friday:
                    dayName = "Freitag";
                    break;
                case DayOfWeek.Saturday:
                    dayName = "Samstag";
                    break;
                case DayOfWeek.Sunday:
                    dayName = "Sonntag";
                    break;
            }

            var monthName = Latex.EncodeLatex(_months[date.Month - 1]);
            return $"{dayName}, der {date.Day}. {monthName} {date.Year}";
        }

        private static string GetTimeString(DateTime date)
        {
            return $"{date.Hour:D2}:{date.Minute:D2}";
        }

        private static string FormatSenderAndTime(IMessage msg)
        {
            var sender = string.Format(@"\textbf{{{0}}}", Latex.EncodeLatex(msg.Sender));
            return string.Format("{0} ({1}):", sender, GetTimeString(msg.Timepoint));
        }

        private string Encode(string str)
        {
            str = Latex.EncodeLatex(str);
            str = Latex.ReplaceURL(str);
            str = _emojis.ReplaceEmojis(str);
            return str;
        }

        private void AppendTextMessage(TextMessage msg, StringBuilder sb)
        {
            var senderAndTime = FormatSenderAndTime(msg);
            var content = Encode(msg.Content);
            sb.AppendLine($"{senderAndTime} {content}");
            sb.AppendLine(@"\\");
        }

        private void AppendImageMessage(ImageMessage msg, StringBuilder sb)
        {
            sb.AppendLine(FormatSenderAndTime(msg) + @"\\");
            sb.AppendLine(@"\begin{center}");
            sb.AppendLine(@"\includegraphics[height=0.1\textheight]{" + Path.Combine(ImageDir, msg.Filename) + @"}\\");
            sb.AppendFormat(@"\small{{\textit{{{0}}}}}", Encode(msg.Subscription));
            sb.AppendLine(@"\end{center}");
        }

        private void AppendMediaOmittedMessage(MediaOmittedMessage msg, StringBuilder sb)
        {
            sb.AppendLine(FormatSenderAndTime(msg) + @"\\");
            sb.AppendLine(@"\begin{center}");
            foreach (var x in msg.Relpaths)
            {
                sb.AppendLine(@"\includegraphics[height=0.1\textheight]{" + Path.Combine(ImagePoolDir, x) + @"}\\");
                sb.AppendFormat(@"\small{{\textit{{{0}}}}}\\", Encode(x));
            }
            sb.AppendLine(@"\end{center}");
        }

        private void AppendMediaMessage(MediaMessage msg, StringBuilder sb)
        {
            var str = string.Format(@"{0} \textit{{{1}}}", FormatSenderAndTime(msg), Latex.EncodeLatex(msg.Filename));
            if (!string.IsNullOrWhiteSpace(msg.Subscription))
            {
                str = str + " - " + Encode(msg.Subscription);
            }

            sb.AppendLine(str);
            sb.AppendLine(@"\\");
        }


        private List<string> ReadEmojiList(string dir)
        {
            var list = new List<string>();
            foreach (var x in Directory.EnumerateFiles(dir))
            {
                var fileName = Path.GetFileName(x);
                list.Add(fileName.Substring(0, fileName.Length - 4));
            }

            return list;
        }
    }
}

