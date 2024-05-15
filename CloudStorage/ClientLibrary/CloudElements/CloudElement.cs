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

        public CloudElement(string path)
        {
            Path = path;
        }
    }
}
