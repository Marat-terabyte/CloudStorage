// MIT License
// Copyright (c) 2024 Marat

using CloudStorageLibrary;
using CloudStorageLibrary.Serializers;
using CloudStorageLibrary.Commands;

namespace CloudStorageLibraryTest
{
    [TestClass]
    public class RequestSerializerTest
    {
        [DataTestMethod]
        [DynamicData(nameof(GetTestRequestsToSerialize), DynamicDataSourceType.Method)]
        public void SerializeTest(Request request, string expectedSerializedRequest)
        {
            string actualSerializedRequest = RequestSerializer.Serialize(request);
            Assert.AreEqual(expectedSerializedRequest, actualSerializedRequest);
        }

        [DataTestMethod]
        [DynamicData(nameof(GetTestRequestsToDeserialize), DynamicDataSourceType.Method)]
        public void DeserializeTest(string request, Request expectedDeserializedRequest)
        {
            Request actualDeserializedRequest = RequestSerializer.Deserialize(request);
            
            Assert.AreEqual(expectedDeserializedRequest, actualDeserializedRequest);
        }

        private static IEnumerable<object[]> GetTestRequestsToSerialize()
        {
            yield return new object[]
            {
                new Request(10,"anonymous", Command.SignIn, ["QWERTY123456"]),
                "Session id: 10\nanonymous\nSignIn\nQWERTY123456",
            };

            yield return new object[]
            {
                new Request("FSAFL/212@FSASFAqwr12()x", Command.Download, ["1251", "", "  ", "SAFASF", "hELl;o", "  "]),
                "Session id: \nFSAFL/212@FSASFAqwr12()x\nDownload\n1251, SAFASF, hELl;o",
            };

            yield return new object[]
            {
                new Request(1, "User", Command.Upload, ["HelloWorld.pdf", "Program.cs", "", " ", "Request.cs", "World.gif", ""]),
                "Session id: 1\nUser\nUpload\nHelloWorld.pdf, Program.cs, Request.cs, World.gif",
            };
        }

        private static IEnumerable<object[]> GetTestRequestsToDeserialize()
        {
            yield return new object[]
            {
                "Session id: 10\nanonymous\nSignIn\nQWERTY123456",
                new Request(10, "anonymous", Command.SignIn, ["QWERTY123456"]),
            };

            yield return new object[]
            {
                "Session id: \nFSAFL/212@FSASFAqwr12()x\nDownload\n1251, SAFASF, hELl;o",
                new Request("FSAFL/212@FSASFAqwr12()x", Command.Download, ["1251", "SAFASF", "hELl;o"]),
            };

            yield return new object[]
            {
                "Session id: 1\nUser\nUpload\nHelloWorld.pdf, Program.cs, Request.cs, World.gif",
                new Request(1, "User", Command.Upload, ["HelloWorld.pdf", "Program.cs", "Request.cs", "World.gif"]),
            };
        }
    }
}
