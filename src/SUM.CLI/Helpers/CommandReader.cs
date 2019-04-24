using SUM.CLI.UI;
using SUM.Core.IO;

namespace SUM.CLI.Helpers
{
	public class CommandReader : ICommandReader
	{
		public ActionDTO Read() =>
			InputHandler.ReadCommandInput();
	}
}
