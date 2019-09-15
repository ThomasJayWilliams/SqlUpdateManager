using System.Collections.Generic;
using System.Linq;

namespace SqlUpdateManager.Core.Data
{
	public class ServerEntity : AbstractEntity
	{
		public override string HashPattern => $"{Name}{Address}";
		public string Address { get; set; }
		public string Username { get; set; }
		public string Password { get; set; }
		public ServerType Type { get; set; }
		public IEnumerable<DatabaseEntity> Databases { get; set; }

		public override IEntity Clone() =>
			new ServerEntity
			{
				Name = string.IsNullOrEmpty(Name) ? null : (string)Name.Clone(),
				Password = string.IsNullOrEmpty(Password) ? null : (string)Password.Clone(),
				Type = Type,
				Username = string.IsNullOrEmpty(Username) ? null : (string)Username.Clone(),
				Databases = Databases == null ? null : Databases.Select(db => (DatabaseEntity)db.Clone()),
				Hash = Hash == null ? null : (byte[])Hash.Clone()
			};
	}
}
