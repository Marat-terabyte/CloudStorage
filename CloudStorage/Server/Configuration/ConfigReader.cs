// MIT License
// Copyright (c) 2024 Marat

using Configuration;
using System.Text.Json;

namespace Server.Configuration
{
    /// <summary>
    /// Reads the configuration file
    /// </summary>
    internal static class ConfigReader
    {
        private static readonly string _configFile = "config.json";

        private static Config? _instance;

        /// <summary>
        /// Reads json from the configuration file
        /// </summary>
        public static Config? ReadConfig()
        {
            if (_instance == null)
                _instance = JsonSerializer.Deserialize<Config>(_configFile);

            return _instance;
        }
    }
}
