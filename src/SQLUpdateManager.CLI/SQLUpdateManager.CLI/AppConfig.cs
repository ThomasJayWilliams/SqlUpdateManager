using Newtonsoft.Json;

namespace SQLUpdateManager.CLI
{
    public class AppConfig
    {
        [JsonProperty("file_encoding")]
        public string FileEncoding { get; set; }
    }
}
