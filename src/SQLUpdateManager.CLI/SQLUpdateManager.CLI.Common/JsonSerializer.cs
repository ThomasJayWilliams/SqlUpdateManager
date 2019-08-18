﻿using Newtonsoft.Json;
using SqlUpdateManager.Core.Common;

namespace SqlUpdateManager.CLI.Common
{
    public class JsonSerializer : ISerializer
    {
        public string Path { get => CLIConstants.RegisterPath; }

        public T Deserialize<T>(string data) =>
            JsonConvert.DeserializeObject<T>(data);

        public string Serialize(object data) =>
            JsonConvert.SerializeObject(data);
    }
}
