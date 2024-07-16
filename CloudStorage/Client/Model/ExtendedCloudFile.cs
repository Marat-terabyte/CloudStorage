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

        public ExtendedCloudFile(string path, string size, string creationTime) : base(path, size, creationTime)
        { 
        }

        public override bool Download(out string? message)
        {
            DownloadCommand command = new DownloadCommand(this);
            
            return command.Execute(out message);
        }

        protected override string GetImageSource()
        {
            string extension = new FileInfo(Path).Extension;
            switch (extension)
            {
                case ".png":
                    return "Assets/png-file.png";
                case ".pdf":
                    return "Assets/pdf-file.png";
                case ".exe":
                    return "Assets/exe-file.png";
                case ".doc":
                    return "Assets/doc-file.png";
                case ".docx":
                    return "Assets/docx-file.png";
                case ".txt":
                    return "Assets/txt-file.png";
                case ".html":
                    return "Assets/html-file.png";
                default:
                    return "Assets/file.png";
            }
        }
    }
}
