using Newtonsoft.Json;

namespace SqlUpdateManager.CLI.Common
{
    public class AppConfig
    {
        [JsonProperty("core")]
        public CoreConfig Core { get; set; }
    }

    public class CoreConfig
    {
        [JsonProperty("fileEncoding")]
        public string FileEncoding { get; set; }
    }
}
