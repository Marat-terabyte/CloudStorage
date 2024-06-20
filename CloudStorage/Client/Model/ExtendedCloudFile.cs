// MIT License
// Copyright (c) 2024 Marat

using ClientLibrary.Commands;
using System.IO;
using System.Windows;

namespace Client.Model
{
    public class ExtendedCloudFile : ExtendedCloudElement
    {
        public ExtendedCloudFile(string path) : base(path)
        {
        }

        public override bool Download(out string? message)
        {
            DownloadCommand command = new DownloadCommand(this);
            
            return command.Execute(out message);
        }
    }
}
