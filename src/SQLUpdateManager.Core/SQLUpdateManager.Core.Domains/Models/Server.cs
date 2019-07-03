using SQLUpdateManager.Core.Internal;
using System.Collections.Generic;

namespace SQLUpdateManager.Core.Domains
{
    public class Server : IData
    {
        public string Name { get; set; }
        public string Location { get; set; }
        public byte[] Hash
        {
            get => Hasher.GetHash($"{Name}{Type}{Location}");
        }
        public IEnumerable<Database> Databases { get; set; }
        public ServerType Type { get; set; }
        public IEnumerable<ServerUser> Users { get; set; }
    }
}
