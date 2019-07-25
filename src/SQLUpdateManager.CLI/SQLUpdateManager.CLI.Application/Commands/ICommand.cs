using System.Collections.Generic;

namespace SQLUpdateManager.CLI.Application
{
	public interface ICommand
	{
        string Name { get; }
        string Argument { get; set; }
        bool RequiresParameters { get; }
        bool RequiresArgument { get; }
        IEnumerable<IParameter> Parameters { get; }

        void Execute();

        void AddParameters(params IParameter[] parameters);
        void AddParameters(IEnumerable<IParameter> parameters);
	}
}
