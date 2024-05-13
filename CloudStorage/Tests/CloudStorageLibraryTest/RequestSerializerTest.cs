// MIT License
// Copyright (c) 2024 Marat

using CloudStorageLibrary;
using CloudStorageLibrary.Serializers;
using CloudStorageLibrary.Commands;
using CloudStorageLibrary.Serializers.Exceptions;

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

        [DataTestMethod]
        public void DeserializeTest_NotSupportedCommand()
        {
            string request = "SessionId: 1\r\nUsername: user\r\nCommand: Sign\r\nArgs: ";

            Assert.ThrowsException<NotSupportedCommand>(() => RequestSerializer.Deserialize(request));
        }

        private static IEnumerable<object[]> GetTestRequestsToSerialize()
        {
            yield return new object[]
            {
                new Request(10,"anonymous", Command.SignIn, ["QWERTY123456"]),
                "SessionId: 10\r\nUsername: anonymous\r\nCommand: SignIn\r\nArgs: QWERTY123456",
            };

            yield return new object[]
            {
                new Request("FSAFL/212@FSASFAqwr12()x", Command.Download, ["1251", "", "  ", "SAFASF", "hELl;o", "  "]),
                "SessionId: \r\nUsername: FSAFL/212@FSASFAqwr12()x\r\nCommand: Download\r\nArgs: 1251, SAFASF, hELl;o",
            };

            yield return new object[]
            {
                new Request(1, "User", Command.Upload, ["HelloWorld.pdf", "Program.cs", "", " ", "Request.cs", "World.gif", ""]),
                "SessionId: 1\r\nUsername: User\r\nCommand: Upload\r\nArgs: HelloWorld.pdf, Program.cs, Request.cs, World.gif",
            };
        }

        private static IEnumerable<object[]> GetTestRequestsToDeserialize()
        {
            yield return new object[]
            {
                "SessionId: 10\r\nUsername: anonymous\r\nCommand: SignIn\r\nArgs: QWERTY123456",
                new Request(10, "anonymous", Command.SignIn, ["QWERTY123456"]),
            };

            yield return new object[]
            {
                "SessionId: \r\nUsername: FSAFL/212@FSASFAqwr12()x\r\nCommand: Download\r\nArgs: 1251, SAFASF, hELl;o",
                new Request("FSAFL/212@FSASFAqwr12()x", Command.Download, ["1251", "SAFASF", "hELl;o"]),
            };

            yield return new object[]
            {
                "SessionId: 1\r\nUsername: User\r\nCommand: Upload\r\nArgs: HelloWorld.pdf, Program.cs, Request.cs, World.gif",
                new Request(1, "User", Command.Upload, ["HelloWorld.pdf", "Program.cs", "Request.cs", "World.gif"]),
            };

            yield return new object[]
            {
                "SessionId: \r\nUsername:  \r\nCommand: Download\r\nArgs:  ",
                new Request() { Command = Command.Download },
            };
        }
    }
}
