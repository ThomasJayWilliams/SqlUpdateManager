namespace SQLUpdateManager.CLI.Application
{
	public interface IParameter
	{
		string Argument { get; set; }
		string Name { get; }
	}
}
