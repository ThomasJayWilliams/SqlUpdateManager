namespace SQLUpdateManager.CLI.Application
{
	public interface IParameter
	{
		string Argument { get; set; }
        bool RequiresArgument { get; }
		string Name { get; }
	}
}
