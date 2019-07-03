using System;
using System.Collections.Generic;
using System.Text;

namespace SQLUpdateManager.CLI.Application
{
	public interface ISUMCommand : ICommand
	{
		IArgument Argument { get; set; }
		IEnumerable<IParameter> Parameters { get; set; }
	}
}
