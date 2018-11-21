using System;
using CommandLine;
using System.IO;
using Helper;

namespace WhatsBookSharp
{
    public class MainClass
    {
        public class Config
        {
            public string InputDir { get; set; }

            public string OutputDir { get; set; }

            public string EmojiDir{ get; set; }
         
            public string ImagePoolDir{ get; set; }

            public string DebugDir{ get; set; }
        }
        
        public class Options
        {
            [Option('i', "inputdir", Required = true, HelpText = "Input directory, should contain a subdirectory 'chat' where conversion and images is stored")]
            public string InputDir { get; set; }

            [Option('o', "outputdir", Required = false, HelpText = "Output directory, default is input directory")]
            public string OutputDir { get; set; }

            [Option('e', "emojidir", Required = true, HelpText = "Directory where the emoji png images are stored")]
            public string EmojiDir{ get; set; }

            [Option("imagepooldir", Required = false, HelpText = "Directory of the image pool. Only used when 'media omitted' messages are found")]
            public string ImagePoolDir{ get; set; }

            [Option("debugdir", Required = false, HelpText = "Directory where debug information is stored")]
            public string DebugDir{ get; set; }
        }

        public static void Main(string[] args)
        {
            Config config;
            // use a config file, for debugging purpose
            if (args[0] == "CONFIGFILE")
            {
                config = Serializer.DeserializeFromXml<Config>(File.ReadAllText(args[1]));
            }
            else
            {
                config = new Config();
                Options options = null;
                Parser.Default.ParseArguments<Options>(args).WithParsed(o => options = o);

                if (options == null)
                {
                    return;
                }

                config.InputDir = options.InputDir;
                config.OutputDir = options.OutputDir;
                config.EmojiDir = options.EmojiDir;
                config.ImagePoolDir = options.ImagePoolDir;
                config.DebugDir = options.DebugDir;
            }

            config.OutputDir = string.IsNullOrWhiteSpace(config.OutputDir) ? config.InputDir : config.OutputDir;

            StreamWriter writer = null;
            if (!string.IsNullOrWhiteSpace(config.DebugDir))
            {
                Directory.CreateDirectory(config.DebugDir);
                writer = new StreamWriter(Path.Combine(config.DebugDir, "output.log"));
                EmojiParser.Debug = writer;
            }

            var creator = new BookCreator(config.InputDir, config.OutputDir, config.EmojiDir);
            creator.ImagePoolDir = config.OutputDir;
            creator.WriteTex();

            EmojiParser.Debug = null;
            writer?.Close();
        }
    }
}
 