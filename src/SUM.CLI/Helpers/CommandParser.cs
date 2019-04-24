using SUM.CLI.Commands;
using SUM.CLI.UI;
using SUM.Core;
using SUM.Core.IO;
using SUM.Core.Managers;

namespace SUM.CLI.Helpers
{
	public class CommandParser : ICommandParser
	{
		public ICommand Parse(ActionDTO data)
		{
			switch (data.Action)
			{
				case InputAction.Exit:
					// DI!!!
					return new ShellExitCommand(
						new ShellExitCommandManager(
							new BinaryGate(
								new InputHandler(),
								new OutputHandler()),
							new OutputHandler()));

				default:
					return null;
			}
		}
	}
}
