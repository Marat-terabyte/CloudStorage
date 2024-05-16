// MIT License
// Copyright (c) 2024 Marat

using ClientLibrary.CloudElements;
using ClientLibrary.CloudElements.Parser;
using CloudStorageLibrary.Commands;
using CloudStorageLibrary;

namespace ClientLibrary.Commands
{
    public class ListCommand : ClientCommand
    {
        public string Path { get; set; }

        public ListCommand(string path)
        {
            Path = path;
        }

        public bool Execute(out IEnumerable<CloudElement>? cloudElements, out string? message)
        {
            cloudElements = null;
            message = null;

            Request request = RequestBuilder.Build(Command.List, [Path]);
            Client.SendRequest(request);

            Response response = Client.ReceiveResponse(out string? data);
            if (response.Status == CommandStatus.NotOk)
            {
                message = data;
                
                return false;
            }

            if (data == null)
                return false;

            cloudElements = new CloudElementParser(data).Parse();

            return true;
        }
    }
}
