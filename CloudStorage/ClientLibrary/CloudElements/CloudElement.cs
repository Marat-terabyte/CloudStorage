// MIT License
// Copyright (c) 2024 Marat

namespace ClientLibrary.CloudElements
{
    public class CloudElement
    {
        public string Name
        {
            get => new FileInfo(Path).Name;
        }
        
        public string Path { get; set; }

        /// <summary>
        /// Size of the file in bytes
        /// </summary>
        public string? Size { get; set; }
        public string? CreationTime { get; set; }

        public CloudElement(string path)
        {
            Path = path;
        }

        public CloudElement(string path, string creationTime) : this(path)
        {
            CreationTime = creationTime;
        }

        public CloudElement(string path, string creationTime, string size) : this(path)
        {
            CreationTime = creationTime;
            Size = size;
        }
    }
}
