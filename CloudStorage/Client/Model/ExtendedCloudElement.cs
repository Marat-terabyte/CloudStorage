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
    /// <summary>
    /// The class that extends <see cref="CloudElement"/> for WPF
    /// </summary>
    public abstract class ExtendedCloudElement : CloudElement
    {
        public string ImageSource { get; set; }

        public ExtendedCloudElement(string path) : base(path)
        {
            ImageSource = "/Client;component/" + GetImageSource();
        }

        public abstract bool Download(out string? message);

        protected abstract string GetImageSource();
    }
}
