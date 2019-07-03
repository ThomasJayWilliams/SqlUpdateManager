namespace SQLUpdateManager.Core.Domains
{
    public class Database : IData
    {
        public string Name { get; set; }
        public string Hash { get; set; }
        public Server Server { get; set; }
        public Procedure[] Procedures { get; set; }
    }
}
