using System.Collections.Generic;

namespace SqlUpdateManager.CLI.Application
{
	public interface ICommand
	{
        string Name { get; }
        string Argument { get; set; }
        IEnumerable<IParameter> Parameters { get; }

        void ValidateAndRun();

        void AddParameters(params IParameter[] parameters);
        void AddParameters(IEnumerable<IParameter> parameters);
	}
}
