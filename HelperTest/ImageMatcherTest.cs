using NUnit.Framework;
using System;
using Helper;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml.Serialization;

namespace HelperTest
{
    [TestFixture()]
    public class ImageMatcherTest
    {
        [Test()]
        public void TestPick_Load_SingleMatch()
        {
            var fileList = new List<ImageMatcher.FileEntry>();
            fileList.Add(GetFileEntry(2015, 1, 1, "file1.jpg"));

            var tp = new DateTime(2015, 1, 1, 13, 12, 0);
            var matchList = new List<ImageMatcher.MatchEntry>();
            var matchEntry = new ImageMatcher.MatchEntry(tp, fileList, 0);
            matchList.Add(matchEntry);
            var matchXML = GetXML(matchList);

            var im = new ImageMatcher();
            im.SearchMode = false;
            im.LoadFromString(matchXML);

            var entry = im.Pick(tp);
            Assert.IsTrue(entry.Timepoint.Equals(tp));

            var fileMatches = entry.Filematches;
            Assert.AreEqual(1, fileMatches.Count());
            Assert.AreEqual("file1.jpg", fileMatches.First().Filename);
        }

        [Test()]
        public void TestPick_Load_NoMatch()
        {
            var fileList = new List<ImageMatcher.FileEntry>();
            fileList.Add(GetFileEntry(2015, 1, 1, "file1.jpg"));

            var tpInput = new DateTime(2015, 1, 1, 13, 12, 0);
            var matchList = new List<ImageMatcher.MatchEntry>();
            var matchEntry = new ImageMatcher.MatchEntry(tpInput, fileList, 0);
            matchList.Add(matchEntry);
            var matchXML = GetXML(matchList);

            var im = new ImageMatcher();
            im.SearchMode = false;
            im.LoadFromString(matchXML);

            try
            {
                var tpSearch = new DateTime(2015, 1, 2, 13, 12, 0);
                im.Pick(tpSearch);
                Assert.Fail("No exception thrown");
            }
            catch (ArgumentException)
            {
            }
        }

        [Test()]
        public void TestPick_FileList_Match()
        {
            var fileList = new List<ImageMatcher.FileEntry>();
            fileList.Add(GetFileEntry(2015, 1, 1, "file1.jpg"));
            fileList.Add(GetFileEntry(2015, 1, 2, "file2.jpg"));
            fileList.Add(GetFileEntry(2015, 1, 3, "file3.jpg"));

            var im = new ImageMatcher();
            im.FileList = fileList;
            im.SearchMode = true;

            var entry = im.Pick(new DateTime(2015, 1, 2, 16, 10, 0), 0);
            Assert.AreEqual(1, entry.Filematches.Count());
            Assert.AreEqual("file2.jpg", entry.Filematches.First().Filename);
        }

        [Test()]
        public void TestPick_FileList_NoMatch()
        {
            var fileList = new List<ImageMatcher.FileEntry>();
            fileList.Add(GetFileEntry(2015, 1, 1, "file1.jpg"));
            fileList.Add(GetFileEntry(2015, 1, 2, "file2.jpg"));
            fileList.Add(GetFileEntry(2015, 1, 3, "file3.jpg"));

            var im = new ImageMatcher();
            im.FileList = fileList;
            im.SearchMode = true;

            var entry = im.Pick(new DateTime(2015, 1, 4, 16, 10, 0), 0);
            Assert.AreEqual(0, entry.Filematches.Count());
        }

        [Test()]
        public void TestPick_FileList_Cnt()
        {
            var fileList = new List<ImageMatcher.FileEntry>();
            fileList.Add(GetFileEntry(2013, 10, 11, "file1.jpg"));
            fileList.Add(GetFileEntry(2013, 10, 11, "file2.jpg"));
            fileList.Add(GetFileEntry(2013, 10, 11, "file3.jpg"));

            var im = new ImageMatcher();
            im.FileList = fileList;
            im.SearchMode = true;

            var entry1 = im.Pick(new DateTime(2013, 10, 11, 8, 25, 0), 0);
            var entry2 = im.Pick(new DateTime(2013, 10, 11, 8, 25, 0), 1);

            Assert.AreEqual(0, entry1.Cnt);
            Assert.AreEqual(1, entry2.Cnt);
            Assert.AreEqual(3, entry1.Filematches.Count());
            Assert.AreEqual(3, entry2.Filematches.Count());
        }

        private ImageMatcher.FileEntry GetFileEntry(int year, int month, int day, string filename)
        {
            var tp = new DateTime(year, month, day);
            var rp = $"path/to/{filename}";
            return new ImageMatcher.FileEntry()
            {
                Timepoint = tp,
                Relpath = rp,
                Filename = filename
            };
        }

        private string GetXML(List<ImageMatcher.MatchEntry> list)
        {
            var sw = new StringWriter();
            var serializer = new XmlSerializer(typeof(List<ImageMatcher.MatchEntry>));
            serializer.Serialize(sw, list);
            return sw.ToString();
        }
    }
}

