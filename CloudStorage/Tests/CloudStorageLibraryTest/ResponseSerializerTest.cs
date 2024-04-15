// MIT License
// Copyright (c) 2024 Marat

using CloudStorageLibrary;
using CloudStorageLibrary.Serializers;
using CloudStorageLibrary.Commands;

namespace CloudStorageLibraryTest
{
    [TestClass]
    public class ResponseSerializerTest
    {
        [DataTestMethod]
        [DynamicData(nameof(GetTestResponseToSerialize), DynamicDataSourceType.Method)]
        public void SerializeTest(Response response, string expectedSerializedResponse)
        {
            string actualSerializedResponse = ResponseSerializer.Serialize(response);

            Assert.AreEqual(expectedSerializedResponse, actualSerializedResponse);
        }

        [DataTestMethod]
        [DynamicData(nameof(GetTestResponseToDeserialize), DynamicDataSourceType.Method)]
        public void DeserializeTest(string response, Response expectedDeserializedResponse)
        {
            Response actualDeserializedResponse = ResponseSerializer.Deserialize(response);

            Assert.AreEqual(expectedDeserializedResponse, actualDeserializedResponse);
        }

        private static IEnumerable<object[]> GetTestResponseToSerialize()
        {
            yield return new object[]
            {
                new Response(CommandStatus.Ok, 1024),
                "Ok\n1024 bytes",
            };

            yield return new object[]
            {
                new Response(CommandStatus.Ok, 10512),
                "Ok\n10512 bytes",
            };

            yield return new object[]
            {
                new Response(CommandStatus.NotOk, 10512),
                "NotOk\n10512 bytes",
            };
        }

        private static IEnumerable<object[]> GetTestResponseToDeserialize()
        {
            yield return new object[]
            {
                "Ok\n1024 bytes",
                new Response(CommandStatus.Ok, 1024),
            };

            yield return new object[]
            {
                "Ok\n10512    bytes  ",
                new Response(CommandStatus.Ok, 10512),
            };

            yield return new object[]
            {
                "NotOk  \n10512   bytes  ",
                new Response(CommandStatus.NotOk, 10512),
            };
        }
    }
}
