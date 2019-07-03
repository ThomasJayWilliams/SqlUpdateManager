namespace SQLUpdateManager.CLI.Application
{
	public interface IParameter
	{
		IArgument Argument { get; set; }
		string Name { get; }
	}
}
