using SQLUpdateManager.Core.Internal;

namespace SQLUpdateManager.Core.Domains
{
    public class StorageProcedure : IData
    {
        public string Name { get; set; }
        public byte[] Hash
        {
            get => Hasher.GetHash($"{Name}{Location}");
        }
        public string Location { get; set; }
    }
}
