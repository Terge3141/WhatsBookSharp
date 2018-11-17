using NUnit.Framework;
using System;
using Helper;

namespace HelperTest
{
	[TestFixture()]
	public class LatexTest
	{
		[Test()]
		public void TestReplaceURL_Http_Only()
		{
			Assert.AreEqual(@"\url{http://ab.cd.ef}", Latex.ReplaceURL("http://ab.cd.ef"));
		}

		[Test()]
		public void TestReplaceURL_Https_Only()
		{
			Assert.AreEqual(@"\url{https://ab.cd.ef/blub}", Latex.ReplaceURL("https://ab.cd.ef/blub"));
		}

		[Test()]
		public void TestReplaceURL_Http_Sandwich()
		{
			Assert.AreEqual(@"aa \url{http://def} ijk", Latex.ReplaceURL("aa http://def ijk"));
		}

		[Test()]
		public void TestReplaceURL_Https_Newline()
		{
			Assert.AreEqual("\\url{https://bla.com/asd}\nabcdef", Latex.ReplaceURL("https://bla.com/asd\nabcdef"));
		}

        [Test()]
        public void TestReplaceURL_Https_LatexNewline()
        {
            Assert.AreEqual("\\url{https://bla.com/asd}\\\\abcdef", Latex.ReplaceURL("https://bla.com/asd\\\\abcdef"));
        }

        [Test()]
        public void TestReplaceURL_Https_LatexEscape()
        {
            Assert.AreEqual("\\url{https://bla.com/asd\\&a=1} abcdef", Latex.ReplaceURL("https://bla.com/asd\\&a=1 abcdef"));
        }

		[Test()]
		public void TestReplaceURL_None()
		{
			Assert.AreEqual("abcdef", Latex.ReplaceURL("abcdef"));
		}

		[Test()]
		public void TestReplaceURL_Https_WordsPrefix()
		{
			Assert.AreEqual(@"Some words \url{https://bla.com}",
				Latex.ReplaceURL("Some words https://bla.com"));
		}

		[Test()]
		public void TestReplaceURL_Http_Words_Https()
		{
			Assert.AreEqual(@"\url{http://as.de} Some other word \url{https://bla.com}",
				Latex.ReplaceURL("http://as.de Some other word https://bla.com"));
		}
	}
}

