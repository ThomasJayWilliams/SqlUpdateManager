namespace SqlUpdateManager.Core.Data
{
	public abstract class AbstractEntity : IEntity
	{
		public abstract string HashPattern { get; }
		public string Name { get; set; }
		public byte[] Hash { get; set; }

		public abstract IEntity Clone();
	}
}
