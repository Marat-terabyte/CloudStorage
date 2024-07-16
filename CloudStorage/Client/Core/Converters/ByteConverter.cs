using System.Globalization;
using System.Windows.Data;

namespace Client.Core.Converters
{
    class ByteConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string? stringSize = value as string;
            if (stringSize != null)
            {
                float.TryParse(stringSize, out float size);
                
                int i;
                for (i = 0; size > 1024; i++)
                    size /= 1024;

                switch (i)
                {
                    case 0:
                        return $"{Math.Ceiling(size)} bytes";
                    case 1:
                        return $"{Math.Ceiling(size)} Kilobytes";
                    case 2:
                        return $"{Math.Ceiling(size)} Megabytes";
                    case 3:
                        return $"{Math.Ceiling(size)} Gigabytes";
                    case 4:
                        return $"{Math.Ceiling(size)} Terabytes";
                }
            }

            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string? stringSize = value as string;
            if (stringSize == null)
                return 0;

            var substrings = stringSize.Split(" ");
            if (substrings.Length < 2)
                return 0;

            int.TryParse(substrings[0], out int size);
            string unitInformation = substrings[1];
            switch (unitInformation)
            {
                case "bytes":
                    return size;
                case "Kilobytes":
                    return size * 1024;
                case "Megabytes":
                    return size * 1024 * 1024;
                case "Gigabytes":
                    return size * 1024 * 1024 * 1024;
                case "Terabytes":
                    return size * 1024 * 1024 * 1024 * 1024;
            }

            return 0;
        }
    }
}
