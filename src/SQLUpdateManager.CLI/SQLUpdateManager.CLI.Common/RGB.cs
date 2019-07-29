using Newtonsoft.Json;

namespace SQLUpdateManager.CLI.Common
{
    public class RGB
    {
        public RGB() { }
        public RGB(byte r, byte g, byte b)
        {
            R = r;
            G = g;
            B = b;
        }

        [JsonProperty("r")]
        public byte R { get; set; }
        [JsonProperty("g")]
        public byte G { get; set; }
        [JsonProperty("b")]
        public byte B { get; set; }
    }
}
