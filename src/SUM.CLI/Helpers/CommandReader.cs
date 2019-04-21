using SUM.CLI.UI;
using SUM.System.IO;

namespace SUM.CLI.Helpers
{
	public class CommandReader : ICommandReader
	{
		private readonly InputHandler handler;
		
		// DI
		public CommandReader(InputHandler handler)
		{
			this.handler = handler;
		}

		public ActionDTO Read() =>
			handler.ReadCommandInput();
	}
}
