namespace SQLUpdateManager.Core.Domains
{
    public class Server : IData
    {
        public string Name { get; set; }
        public string Hash { get; set; }
        public Database[] Databases { get; set; }
        public ServerType Type { get; set; }
        public ServerUser[] Users { get; set; }
    }
}
