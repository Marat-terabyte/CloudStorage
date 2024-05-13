// MIT License
// Copyright (c) 2024 Marat

using System.Text;

namespace CloudStorageLibrary.Serializers
{
    internal class StringSeparator
    {
            public static string Separate(ref int index, string str, char separator)
            {
                StringBuilder sb = new StringBuilder();
                while (index < str.Length && str[index] != separator)
                {
                    sb.Append(str[index++]);
                }

                index++;

                return sb.ToString();
            }

        public static string Separate(ref int index, string str, string separator)
        {
            StringBuilder sb = new StringBuilder();

            for (; index < str.Length; index++)
            {
                for (int j = 0; j < separator.Length; j++)
                {
                    if (str[index + j] != separator[j])
                        break;

                    if (j == separator.Length - 1)
                    {
                        index += separator.Length;
                        return sb.ToString();
                    }
                }

                sb.Append(str[index]);
            }

            return sb.ToString();
        }
    }
}
