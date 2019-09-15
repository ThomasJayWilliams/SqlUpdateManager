using System.Collections.Generic;
using System.Linq;

namespace SqlUpdateManager.Core.Data
{
	public class DatabaseEntity : AbstractEntity
	{
		public override string HashPattern => $"{Name}";

		public IEnumerable<ProcedureEntity> Procedures { get; set; }

		public override IEntity Clone() =>
			new DatabaseEntity
			{
				Name = string.IsNullOrEmpty(Name) ? null : (string)Name.Clone(),
				Procedures = Procedures == null ? null : Procedures.Select(p => (ProcedureEntity)p.Clone()),
				Hash = Hash == null ? null : (byte[])Hash.Clone()
			};
	}
}
