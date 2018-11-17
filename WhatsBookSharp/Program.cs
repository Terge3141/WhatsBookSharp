using System;
using System.IO;
using System.Text;
using System.Globalization;
using System.Text.RegularExpressions;
using MessageParser;
using Helper;
using System.Collections.Generic;
using System.Linq;

namespace WhatsBookSharp
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            var writer = new StreamWriter("/tmp/output.log");
            EmojiParser.Debug = writer;

            var dir = "/tmp/chat";
            var imagePoolDir = "/tmp/imagepool";
            var emojiDir = "/tmp/blub/emojis";

            var creator = new BookCreator(dir, dir, emojiDir);
            creator.ImagePoolDir = imagePoolDir;
            creator.WriteTex();

            EmojiParser.Debug = null;
            writer.Close();
        }
    }
}
 