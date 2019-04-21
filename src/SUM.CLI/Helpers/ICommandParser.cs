using SUM.CLI.Core.Commands;
using SUM.System.IO;

namespace SUM.CLI.Helpers
{
	public interface ICommandParser
	{
		ICommand Parse(ActionDTO data);
	}
}
