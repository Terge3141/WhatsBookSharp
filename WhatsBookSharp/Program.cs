using System;
using CommandLine;
using System.IO;
using Helper;

namespace WhatsBookSharp
{
    class MainClass
    {
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
            Options options = null;
            Parser.Default.ParseArguments<Options>(args).WithParsed(o => options = o);

            if (options == null)
            {
                return;
            }

            var inputDir = options.InputDir;
            var outputDir = (options.OutputDir == null) ? inputDir : options.OutputDir;
            var emojiDir = options.EmojiDir;
            var imagePoolDir = options.ImagePoolDir;

            var debugDir = options.DebugDir;
            StreamWriter writer = null;
            if (debugDir != null)
            {
                Directory.CreateDirectory(debugDir);
                writer = new StreamWriter(Path.Combine(debugDir, "output.log"));
                EmojiParser.Debug = writer;
            }

            var creator = new BookCreator(inputDir, outputDir, emojiDir);
            creator.ImagePoolDir = imagePoolDir;
            creator.WriteTex();

            EmojiParser.Debug = null;
            writer?.Close();
        }
    }
}
 