﻿// MIT License
// Copyright (c) 2024 Marat

using ClientLibrary.CloudElements;
using ClientLibrary.Commands;
using System.IO;
using System.Windows.Input;

namespace Client.Model
{
    public class ExtendedCloudFolder : ExtendedCloudElement
    {
        public ExtendedCloudFolder(string path) : base(path)
        {
        }

        public ExtendedCloudFolder(string path, string creationTime) : base(path, creationTime)
        {
        }

        public override bool Download(out string? message)
        {
            ListCommand command = new ListCommand(Path);

            bool isSuccess = command.Execute(out IEnumerable<CloudElement>? elements, out message);
            if (!isSuccess)
                return false;

            if (elements == null)
                return false;

            foreach (var element in elements)
            {
                if (element is CloudFile cloudFile)
                {
                    var path = cloudFile.Path;
                    new ExtendedCloudFile(path).Download(out message);
                }
                else if (element is CloudFolder cloudDir)
                {
                    var path = cloudDir.Path;
                    if (element.Name != "..")
                        new ExtendedCloudFolder(path).Download(out message);
                }

            }

            return true;
        }

        protected override string GetImageSource() => "Assets/folder.png";
    }
}
