// MIT License
// Copyright (c) 2024 Marat

using CloudStorageLibrary;

namespace ClientLibrary.CloudElements
{
    public class CloudFolder : CloudElement
    {
        public List<CloudElement> CloudElements { get; set; }

        public CloudFolder(string path) : base(path)
        {
            CloudElements = new List<CloudElement>();
        }

        public CloudFolder(string path, string creationTime) : base(path, creationTime)
        {
            CloudElements = new List<CloudElement>();
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as CloudFolder);
        }

        public bool Equals(CloudFolder? other)
        {
            return other != null &&
                other.Path == Path &&
                other.Name == Name &&
                Enumerable.SequenceEqual(CloudElements, other.CloudElements);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Path, Name, CloudElements);
        }
    }
}
