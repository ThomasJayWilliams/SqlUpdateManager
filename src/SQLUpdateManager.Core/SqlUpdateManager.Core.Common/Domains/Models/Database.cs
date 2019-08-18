using System.Collections.Generic;

namespace SqlUpdateManager.Core.Common
{
	public class Database : IData
    {
        public string Name { get; set; }
        public byte[] Hash
        {
            get => Hasher.GetHash($"{Name}");
        }
        public IEnumerable<Procedure> Procedures { get; set; }

        public override string ToString() =>
            $"{Name}";
    }
}
