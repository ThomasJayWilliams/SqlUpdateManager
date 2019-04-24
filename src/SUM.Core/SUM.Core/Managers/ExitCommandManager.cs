using System;

namespace SUM.Core.Managers
{
	public class ExitCommandManager : BaseManager, IManager
	{
		private readonly IBinaryGate gate = new BinaryGate();

		public void Execute()
		{
			var gateResult = gate.Request("Are you sure you want to exit?");

			if (gateResult)
				Environment.Exit(0);

			else
				OnOutput("Operation has been cancelled!");
		}
	}
}
