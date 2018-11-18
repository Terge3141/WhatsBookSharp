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
            var parser = new EmojiParser(list, x => GetIcon(x));
            var ch = Char.ConvertFromUtf32(0x1f55e);
            var input = "" + ch;
            var output = parser.ReplaceEmojis(input);
            Assert.AreEqual(input, output);
        }

        [Test()]
        public void TestReplaceEmojis_Single_InList1()
        {
            var list = new List<string>() { "1f55e" };
            var parser = new EmojiParser(list, x => GetIcon(x));
            var ch = Char.ConvertFromUtf32(0x1f55e);
            var input = "" + ch;
            var output = parser.ReplaceEmojis(input);
            Assert.AreEqual("ICON(1f55e)", output);
        }

        [Test()]
        public void TestReplaceEmojis_Single_InList2()
        {
            var list = new List<string>(){ "1f4aa", "1f4aa_1f3fb" };
            var parser = new EmojiParser(list, x => GetIcon(x));
            var input = Char.ConvertFromUtf32(0x1f4aa);
            var output = parser.ReplaceEmojis(input);
            Assert.AreEqual("ICON(1f4aa)", output);
        }

        [Test()]
        public void TestReplaceEmojis_Double_NotInList()
        {
            var list = new List<string>();
            var parser = new EmojiParser(list, x => GetIcon(x));
            var input = Char.ConvertFromUtf32(0x1f4a8) + Char.ConvertFromUtf32(0x1f4a9);
            var output = parser.ReplaceEmojis(input);
            Assert.AreEqual(input, output);
        }

        [Test()]
        public void TestReplaceEmojis_Double_InList1()
        {
            var list = new List<string>(){ "1f4aa_1f3fb" };
            var parser = new EmojiParser(list, x => GetIcon(x));
            var input = Char.ConvertFromUtf32(0x1f4aa) + Char.ConvertFromUtf32(0x1f3fb);
            var output = parser.ReplaceEmojis(input);
            Assert.AreEqual("ICON(1f4aa_1f3fb)", output);
        }

        [Test()]
        public void TestReplaceEmojis_Double_InList2()
        {
            var list = new List<string>(){ "1f4aa", "1f4aa_1f3fb" };
            var parser = new EmojiParser(list, x => GetIcon(x));
            var input = Char.ConvertFromUtf32(0x1f4aa) + Char.ConvertFromUtf32(0x1f3fb);
            var output = parser.ReplaceEmojis(input);
            Assert.AreEqual("ICON(1f4aa_1f3fb)", output);
        }

        [Test()]
        public void TestReplaceEmojis_List1()
        {
            var list = new List<string>(){ "1f4aa" };
            var parser = new EmojiParser(list, x => GetIcon(x));
            var input = "abcde";
            var output = parser.ReplaceEmojis(input);
            Assert.AreEqual(input, output);
        }

        [Test()]
        public void TestReplaceEmojis_List2()
        {
            var list = new List<string>() { "1f4aa" };
            var parser = new EmojiParser(list, x => GetIcon(x));
            var input = "abcdef" + Char.ConvertFromUtf32(0x1f4aa) + "GHIJKLM";
            var output = parser.ReplaceEmojis(input);
            Assert.AreEqual("abcdefICON(1f4aa)GHIJKLM", output);
        }

        [Test()]
        public void TestReplaceEmojis_NormalCharAndEmoji()
        {
            // # and 0x20e3 will result in 0023-20e3 (#=0023)
            var list = new List<string>(){ "0023_20e3" };
            var parser = new EmojiParser(list, x => GetIcon(x));
            var input = "#" + Char.ConvertFromUtf32(0x20e3);
            var output = parser.ReplaceEmojis(input);
            Assert.AreEqual("ICON(0023_20e3)", output);
        }

        private string GetIcon(string hex)
        {
            return $"ICON({hex})";
        }
    }
}

