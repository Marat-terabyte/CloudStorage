// MIT License
// Copyright (c) 2024 Marat

using System.Text;

namespace ClientLibrary.CloudElements.Parser
{
    

    public class CloudElementParser
    {
        private int _index;

        public string Paths { get; set; }

        public CloudElementParser(string paths)
        {
            Paths = paths;
            _index = 0;
        }

        public IEnumerable<CloudElement> Parse()
        {
            List<CloudElement> cloudElements = new List<CloudElement>();

            if (Paths == null || Paths.Length == 0)
                return Enumerable.Empty<CloudElement>();

            string line = ReadLine();
            while (!string.IsNullOrEmpty(line))
            {
                string type = ReadType(line, out int i);
                string path = ReadPath(line, i += 1);

                if (type == "dir")
                    cloudElements.Add(new CloudFolder(path));
                else if (type == "file")
                    cloudElements.Add(new CloudFile(path));

                line = ReadLine();
            }

            return cloudElements;
        }

        /// <summary>Reads <paramref name="line"/> for a type</summary>
        /// <param name="line">The line to read</param>
        /// <param name="index">Where a type ends</param>
        /// <returns>Type of a cloud element</returns>
        private string ReadType(string line, out int index)
        {
            StringBuilder type = new StringBuilder();

            index = 0;
            while (index < line.Length && line[index] != ':')
            {
                type.Append(line[index++]);
            }

            return type.ToString().Trim();
        }

        private string ReadPath(string line, int index)
        {
            StringBuilder path = new StringBuilder();
            while (line.Length > index && line[index] != '\n')
            {
                path.Append(line[index++]);
            }

            return path.ToString().Trim();
        }

        private string ReadLine()
        {
            StringBuilder stringBuilder = new StringBuilder();

            while (_index < Paths.Length && Paths[_index] != '\n')
            {
                stringBuilder.Append(Paths[_index++]);
            }

            _index++;

            return stringBuilder.ToString();
        }
    }
}
