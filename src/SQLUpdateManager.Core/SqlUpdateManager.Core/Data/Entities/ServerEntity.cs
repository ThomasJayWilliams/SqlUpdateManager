using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace SqlUpdateManager.Core.Data
{
	public class ServerEntity : IEntity
	{
		public string Name { get; set; }
		public byte[] Hash
		{
			get => Hasher.GetHash($"{Name}{Address}");
			set => Hash = value;
		}
		public string Address { get; set; }
		public string Username { get; set; }
		public string Password { get; set; }
		public ServerType Type { get; set; }
		public IEnumerable<DatabaseEntity> Databases { get; set; }

		public IEntity Clone() =>
			new ServerEntity
			{
				Name = (string)Name.Clone(),
				Password = (string)Password.Clone(),
				Type = Type,
				Username = (string)Username.Clone(),
				Databases = Databases.Select(db => (DatabaseEntity)db.Clone())
			};
	}
}
