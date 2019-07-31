using Newtonsoft.Json;
using System.Collections.Generic;

namespace SQLUpdateManager.CLI.Common
{
    public class Storage
    {
        [JsonProperty("servers")]
        public IEnumerable<StorageServer> Servers { get; set; }
    }

    public class StorageServer
    {
        [JsonProperty("alias")]
        public string Alias { get; set; }

        [JsonProperty("serverName")]
        public string Name { get; set; }

        [JsonProperty("serverLocation")]
        public string Location { get; set; }

        [JsonProperty("serverUsername")]
        public string Username { get; set; }
    }
}
