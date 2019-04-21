using SUM.CLI.Core.Commands;
using SUM.System;

namespace SUM.CLI.Helpers
{
	public interface ICommandParser
	{
		ICommand Parse(InputCommand data);
	}
}
