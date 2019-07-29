using Newtonsoft.Json;

namespace SQLUpdateManager.CLI.Common
{
    public class RGB
    {
        private static readonly RGB _noColor = new RGB(0, 0, 0);

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

        public static RGB NoColor { get => _noColor; }
    }
}
