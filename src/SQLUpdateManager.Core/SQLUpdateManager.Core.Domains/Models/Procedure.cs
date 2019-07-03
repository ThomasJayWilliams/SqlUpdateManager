namespace SQLUpdateManager.Core.Domains
{
    public class Procedure : IData
    {
        public string Name { get; set; }
        public string Hash { get; set; }
        public string Data { get; set; }
        public string Location { get; set; }
        public Database Database { get; set; }
    }
}
