using SUM.Core.Managers;

namespace SUM.CLI.Commands
{
	public class ShellExitCommand : IShellCommand
	{
		private readonly IManager manager;

		// Replace with Ninject injection
		public ShellExitCommand(IManager manager)
		{
			this.manager = manager;
		}

		public void Invoke() =>
			manager.Execute();
	}
}
