namespace SqlUpdateManager.Core.Data
{
	public interface IEntity
	{
		string Name { get; set; }
		byte[] Hash { get; set; }
		string HashPattern { get; }

		IEntity Clone();
	}
}
