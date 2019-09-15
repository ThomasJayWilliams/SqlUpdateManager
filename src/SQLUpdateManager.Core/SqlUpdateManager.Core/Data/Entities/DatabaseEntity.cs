using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace SqlUpdateManager.Core.Data
{
	public class DatabaseEntity : IEntity
	{
		public string Name { get; set; }
		public byte[] Hash
		{
			get => Hasher.GetHash($"{Name}");
			set => Hash = value;
		}
		public IEnumerable<ProcedureEntity> Procedures { get; set; }

		public IEntity Clone() =>
			new DatabaseEntity
			{
				Name = (string)Name.Clone(),
				Procedures = Procedures.Select(p => (ProcedureEntity)p.Clone())
			};
	}
}
