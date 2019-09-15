using Newtonsoft.Json;

namespace SqlUpdateManager.Core.Data
{
	public class ProcedureEntity : IEntity
	{
		public string Name { get; set; }
		public string Path { get; set; }
		public byte[] Hash
		{
			get => Hasher.GetHash($"{Name}{Path}");
			set => Hash = value;
		}
		public string Content { get; set; }

		public IEntity Clone() =>
			new ProcedureEntity
			{
				Path = (string)Path.Clone(),
				Name = (string)Name.Clone()
			};
	}
}
