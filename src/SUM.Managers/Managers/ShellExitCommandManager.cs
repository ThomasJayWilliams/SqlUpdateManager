using SUM.Gates;
using SUM.System.IO;
using System;

namespace SUM.Managers
{
	public class ShellExitCommandManager : IShellCommandManager
	{
		private readonly IBinaryGate gate;
		private readonly IOutputHandler oHandler;

		public ShellExitCommandManager(IBinaryGate gate, IOutputHandler oHandler)
		{
			this.gate = gate;
			this.oHandler = oHandler;
		}

		public void Execute()
		{
			var gateResult = gate.Request("Are you sure you want to exit?");

			if (gateResult)
				Environment.Exit(0);

			else
				oHandler.Out("Operation has been cancelled!");
		}
	}
}
