using SQLUpdateManager.Core.Internal;
using System.Collections.Generic;

namespace SQLUpdateManager.Core.Domains
{
    public class Database : IData
    {
        public string Name { get; set; }
        public byte[] Hash
        {
            get => Hasher.GetHash($"{Name}");
        }
        public IEnumerable<Procedure> Procedures { get; set; }
    }
}
