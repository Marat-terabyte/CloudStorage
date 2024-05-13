// MIT License
// Copyright (c) 2024 Marat

using Configuration;
using System.Text.Json;
using Server.Configuration.Exceptions;

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
        /// <exception cref="ConfigFileNotFound"></exception>
        public static Config ReadConfig()
        {
            if (_instance == null)
            {
                if (File.Exists(_configFile))
                    _instance = JsonSerializer.Deserialize<Config>(File.ReadAllText(_configFile));
                else
                    throw new ConfigFileNotFound($"{_configFile} not found");
            }

            return _instance!;
        }
    }
}
