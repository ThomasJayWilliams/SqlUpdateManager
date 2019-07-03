using System.Collections.Generic;

namespace SQLUpdateManager.Core.Domains
{
    public class Server : IData
    {
        public string Name { get; set; }
        public string Hash { get; set; }
        public IEnumerable<Database> Databases { get; set; }
        public ServerType Type { get; set; }
        public IEnumerable<ServerUser> Users { get; set; }
    }
}
