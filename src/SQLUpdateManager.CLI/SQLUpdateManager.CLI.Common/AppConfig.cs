using Newtonsoft.Json;

namespace SQLUpdateManager.CLI.Common
{
    public class AppConfig
    {
        [JsonProperty("core")]
        public CoreConfig Core { get; set; }

        [JsonProperty("colorTheme")]
        public string Theme { get; set; }
    }

    public class CoreConfig
    {
        [JsonProperty("fileEncoding")]
        public string FileEncoding { get; set; }
    }
}
