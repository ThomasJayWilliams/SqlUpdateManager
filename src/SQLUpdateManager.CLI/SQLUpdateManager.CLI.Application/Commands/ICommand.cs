using System.Collections.Generic;

namespace SQLUpdateManager.CLI.Application
{
	public interface ICommand
	{
        string Name { get; }
        string Argument { get; set; }
        bool HasParameters { get; }
        bool RequiresArgument { get; }
        IEnumerable<IParameter> Parameters { get; set; }

        void Execute();
	}
}
