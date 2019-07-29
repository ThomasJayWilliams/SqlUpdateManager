using Newtonsoft.Json;

namespace SQLUpdateManager.CLI.Common
{
    public class ConsoleTheme
    {
        [JsonProperty("name")]
        public string ThemeName { get; set; }
        [JsonProperty("errorColor")]
        public RGB ErrorColor { get; set; }
        [JsonProperty("serverColor")]
        public RGB ServerColor { get; set; }
        [JsonProperty("databaseColor")]
        public RGB DatabaseColor { get; set; }
        [JsonProperty("procedureColor")]
        public RGB ProcedureColor { get; set; }
        [JsonProperty("appColor")]
        public RGB AppColor { get; set; }
    }
}
