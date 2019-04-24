using SUM.Core;
using SUM.Core.IO;
using System;

namespace SUM.CLI.UI
{
	public class InputHandler : IInputHandler
	{
		public ActionDTO ReadCommandInput() =>
			CommandConverter.Convert(Console.ReadLine());

		public InputGate ReadGateInput() =>
			GateConverter.Convert(Console.ReadLine());
	}
}
