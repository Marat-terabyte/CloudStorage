// MIT License
// Copyright (c) 2024 Marat

using ClientLibrary.CloudElements;
using ClientLibrary.CloudElements.Parser;

namespace ClientLibraryTest
{
    [TestClass]
    public class ParserTest
    {
        [DataTestMethod]
        [DynamicData(nameof(GetTestPathsToParse), DynamicDataSourceType.Method)]
        public void ParseTest(string stringToParse, IEnumerable<CloudElement> expected)
        {
            IEnumerable<CloudElement> actual = new CloudElementParser(stringToParse).Parse();
            Assert.IsTrue(Enumerable.SequenceEqual(expected, actual));
        }

        private static IEnumerable<object[]> GetTestPathsToParse()
        {
            yield return new object[]
            {
                "dir: ..\ndir: directory\nfile: file.txt\nfile: text.txt\nfile: program.exe",
                new List<CloudElement> { new CloudFolder(".."), new CloudFolder("directory"), new CloudFile("file.txt"), new CloudFile("text.txt"), new CloudFile("program.exe") },
            };

            yield return new object[]
            {
                "dir:dir\nfile: file.txt\nfile:text.txt\nfileprogram.exe",
                new List<CloudElement> { new CloudFolder("dir"), new CloudFile("file.txt"), new CloudFile("text.txt") },
            };

            yield return new object[]
            {
                "dir:   dir\n  file :  file.txt\nfile :text.txt\n fileprogram.exe",
                new List<CloudElement> { new CloudFolder("dir"), new CloudFile("file.txt"), new CloudFile("text.txt") },
            };
        }
    }
}