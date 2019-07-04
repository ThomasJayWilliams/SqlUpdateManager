using System.Collections.Generic;

namespace SQLUpdateManager.CLI.Application
{
	public interface ICommand
	{
        string Name { get; }
        IArgument Argument { get; set; }
        IEnumerable<IParameter> Parameters { get; set; }

        void Execute();
	}
}
