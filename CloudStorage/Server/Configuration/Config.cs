// MIT License
// Copyright (c) 2024 Marat

namespace Configuration
{
    /// <summary>
    /// Contains configuration file values
    /// </summary>
    internal class Config
    {
        public string? Host { get; set; }
        public int? MainPort { get; set; }
        public int? DataTransferPort { get; set; }
        public string? DatabaseConnectionString {  get; set; }
    }
}
