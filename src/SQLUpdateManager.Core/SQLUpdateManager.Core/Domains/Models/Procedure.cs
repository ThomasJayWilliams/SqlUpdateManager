using SQLUpdateManager.Core.Internal;
using System.IO;

namespace SQLUpdateManager.Core.Domains
{
    public class Procedure : IData
	{
		private string _location;

		public string Name { get; set; }
		public byte[] Hash
		{
			get => Hasher.GetHash($"{Name}{Location}");
		}
		public string Location
		{
			get => _location;
			set
			{
				if (Path.GetExtension(value) != ".sql")
					throw new InvalidDataException($"The specified procedure {value} should have .sql extension!");
				if (!File.Exists(value))
					throw new FileNotFoundException($"{value} procedure does not exist!");
				_location = value;
			}
        }

        public override string ToString() =>
            $"{Name} {Location}";
    }
}
