// MIT License
// Copyright (c) 2024 Marat

namespace ClientLibrary.CloudElements
{
    public class CloudFile : CloudElement
    {
        public string FileExtension { get => new FileInfo(Name).Extension; }

        public CloudFile(string path) : base(path)
        {
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as CloudFile);
        }

        public bool Equals(CloudFile? other)
        {
            return other != null &&
                other.Path == Path &&
                other.Name == Name &&
                other.FileExtension == FileExtension;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Path, Name, FileExtension);
        }
    }
}
