using SUM.CLI.Core.Commands;
using SUM.CLI.Core.Commands.ShellCommands;
using SUM.CLI.UI;
using SUM.Gates;
using SUM.Managers;
using SUM.System;
using SUM.System.IO;

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
