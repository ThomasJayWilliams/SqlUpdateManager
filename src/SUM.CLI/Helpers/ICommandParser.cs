using SUM.CLI.Commands;
using SUM.Core.IO;

namespace SUM.CLI.Helpers
{
	public interface ICommandParser
	{
		ICommand Parse(ActionDTO data);
	}
}
