using NUnit.Framework;
using System;
using System.Collections.Generic;
using Helper;

namespace HelperTest
{
    [TestFixture()]
    public class EmojiParserTest
    {
        [Test()]
        public void TestReplaceEmojis_Single_NotInList()
        {
            var list = new List<string>();
            var parser = new EmojiParser(list, "ICON{0}");
            var ch = Char.ConvertFromUtf32(0x1F55E);
            var input = "" + ch;
            var output = parser.ReplaceEmojis(input);
            Assert.AreEqual(input, output);
        }

        [Test()]
        public void TestReplaceEmojis_Single_InList1()
        {
            var list = new List<string>() { "1F55E" };
            var parser = new EmojiParser(list, "ICON{0}");
            var ch = Char.ConvertFromUtf32(0x1F55E);
            var input = "" + ch;
            var output = parser.ReplaceEmojis(input);
            Assert.AreEqual("ICON1F55E", output);
        }

        [Test()]
        public void TestReplaceEmojis_Single_InList2()
        {
            var list = new List<string>(){ "1F4AA", "1F4AA-1F3FB" };
            var parser = new EmojiParser(list, "ICON{0}");
            var input = Char.ConvertFromUtf32(0x1F4AA);
            var output = parser.ReplaceEmojis(input);
            Assert.AreEqual("ICON1F4AA", output);
        }

        [Test()]
        public void TestReplaceEmojis_Double_NotInList()
        {
            var list = new List<string>();
            var parser = new EmojiParser(list, "ICON{0}");
            var input = Char.ConvertFromUtf32(0x1F4A8) + Char.ConvertFromUtf32(0x1F4A9);
            var output = parser.ReplaceEmojis(input);
            Assert.AreEqual(input, output);
        }

        [Test()]
        public void TestReplaceEmojis_Double_InList1()
        {
            var list = new List<string>(){ "1F4AA-1F3FB" };
            var parser = new EmojiParser(list, "ICON{0}");
            var input = Char.ConvertFromUtf32(0x1F4AA) + Char.ConvertFromUtf32(0x1F3FB);
            var output = parser.ReplaceEmojis(input);
            Assert.AreEqual("ICON1F4AA-1F3FB", output);
        }

        [Test()]
        public void TestReplaceEmojis_Double_InList2()
        {
            var list = new List<string>(){ "1F4AA", "1F4AA-1F3FB" };
            var parser = new EmojiParser(list, "ICON{0}");
            var input = Char.ConvertFromUtf32(0x1F4AA) + Char.ConvertFromUtf32(0x1F3FB);
            var output = parser.ReplaceEmojis(input);
            Assert.AreEqual("ICON1F4AA-1F3FB", output);
        }

        [Test()]
        public void TestReplaceEmojis_List1()
        {
            var list = new List<string>(){ "1F4AA" };
            var parser = new EmojiParser(list, "ICON{0}");
            var input = "abcde";
            var output = parser.ReplaceEmojis(input);
            Assert.AreEqual(input, output);
        }

        [Test()]
        public void TestReplaceEmojis_List2()
        {
            var list = new List<string>() { "1F4AA" };
            var parser = new EmojiParser(list, "ICON({0})");
            var input = "abcdef" + Char.ConvertFromUtf32(0x1F4AA) + "GHIJKLM";
            var output = parser.ReplaceEmojis(input);
            Assert.AreEqual("abcdefICON(1F4AA)GHIJKLM", output);
        }

        [Test()]
        public void TestReplaceEmojis_NormalCharAndEmoji()
        {
            // # and 0x20E3 will result in 0023-20E3 (#=0023)
            var list = new List<string>(){ "0023-20E3" };
            var parser = new EmojiParser(list, "ICON({0})");
            var input = "#" + Char.ConvertFromUtf32(0x20E3);
            var output = parser.ReplaceEmojis(input);
            Assert.AreEqual("ICON(0023-20E3)", output);
        }
    }
}

