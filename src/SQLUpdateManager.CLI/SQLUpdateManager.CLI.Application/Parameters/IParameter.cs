namespace SQLUpdateManager.CLI.Application
{
	public interface IParameter
	{
		IArgument Argument { get; set; }
        bool HasArgument { get; }
		string Name { get; }
        string Single { get; }
        string Double { get; }
	}
}
