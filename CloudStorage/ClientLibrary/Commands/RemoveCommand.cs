// MIT License
// Copyright (c) 2024 Marat

using ClientLibrary.CloudElements;
using CloudStorageLibrary;
using CloudStorageLibrary.Commands;

namespace ClientLibrary.Commands
{
    public class RemoveCommand : ClientCommand
    {
        public List<CloudElement> CloudElements { get; set; }

        public RemoveCommand(IEnumerable<CloudElement> elements)
        {
            CloudElements = elements.ToList();
        }

        public RemoveCommand(List<CloudElement> elements)
        {
            CloudElements = elements;
        }

        public bool Execute(out string? message)
        {
            message = null;
            
            List<string> paths = new List<string>();
            foreach (CloudElement element in CloudElements)
                paths.Add(element.Path);

            Request request = RequestBuilder.Build(Command.Remove, paths.ToArray());
            Client.SendRequest(request);

            Response response = Client.ReceiveResponse(out string? data);
            message = data;

            if (response.Status == CommandStatus.Ok)
                return true;
            else 
                return false;
        }
    }
}
