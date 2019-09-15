namespace SqlUpdateManager.Core.Data
{
	public class ProcedureEntity : AbstractEntity
	{
		public override string HashPattern => $"{Name}{Path}";
		public string Path { get; set; }
		public byte[] ContentHash { get; set; }

		public override IEntity Clone() =>
			new ProcedureEntity
			{
				Path = string.IsNullOrEmpty(Path) ? null : (string)Path.Clone(),
				Name = string.IsNullOrEmpty(Name) ? null : (string)Name.Clone(),
				Hash = Hash == null ? null : (byte[])Hash.Clone()
			};
	}
}
