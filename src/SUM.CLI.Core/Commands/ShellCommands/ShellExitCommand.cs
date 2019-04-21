using SUM.Managers;

namespace SUM.CLI.Core.Commands.ShellCommands
{
	public class ShellExitCommand : IShellCommand
	{
		private readonly IShellCommandManager manager;

		// Replace with Ninject injection
		public ShellExitCommand(IShellCommandManager manager)
		{
			this.manager = manager;
		}

		public void Invoke() =>
			manager.Execute();
	}
}
