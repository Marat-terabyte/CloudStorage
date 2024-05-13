using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Configuration.Exceptions
{
    internal class ConfigFileNotFound : Exception
    {
        public ConfigFileNotFound() { }

        public ConfigFileNotFound(string message) : base(message) { }

        public ConfigFileNotFound(string message, Exception innerException) : base(message, innerException) { }
    }
}
