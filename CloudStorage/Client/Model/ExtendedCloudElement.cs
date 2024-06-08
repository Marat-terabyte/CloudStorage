// MIT License
// Copyright (c) 2024 Marat

using System.ComponentModel;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Input;
using ClientLibrary.CloudElements;

namespace Client.Model
{
    public abstract class ExtendedCloudElement : CloudElement
    {
        public string ImageSource { get; set; }

        public ExtendedCloudElement(string path) : base(path)
        {
            ImageSource = "/Client;component/" + GetImageSource();
        }

        public abstract bool Download(out string? message);

        protected virtual string GetImageSource()
        {
            switch (new FileInfo(Path).Extension)
            {
                case "":
                    return "Assets/folder.png";
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
