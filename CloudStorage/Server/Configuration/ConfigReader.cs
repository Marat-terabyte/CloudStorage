// MIT License
// Copyright (c) 2024 Marat

using Configuration;
using System.Text.Json;

namespace Server.Configuration
{
    /// <summary>
    /// Reads the configuration file
    /// </summary>
    internal class ConfigReader
    {
        private readonly string _configFile = "config.json";

        /// <summary>
        /// Reads json from the configuration file
        /// </summary>
        public Config? ReadConfig()
        {
            return JsonSerializer.Deserialize<Config>(File.ReadAllText(_configFile));
        }
    }
}
