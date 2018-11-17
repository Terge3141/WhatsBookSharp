using System;
using System.IO;

namespace Helper
{
    public class FileList
    {
        public List<FileEntry> Files
        {
            get;
            set;
        }
        
        public FileList()
        {
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
    }
}

