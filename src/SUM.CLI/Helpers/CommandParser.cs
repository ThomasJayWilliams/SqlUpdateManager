using SUM.CLI.Core.Commands;
using SUM.CLI.Core.Commands.ShellCommands;
using SUM.CLI.UI;
using SUM.Gates;
using SUM.Managers;
using SUM.System;

namespace SUM.CLI.Helpers
{
	public class CommandParser : ICommandParser
	{
		public ICommand Parse(InputCommand data)
		{
			switch (data)
			{
				case InputCommand.Exit:
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
