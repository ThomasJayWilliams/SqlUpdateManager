using SUM.System;
using SUM.System.IO;
using SUM.CLI.UI.Converters;
using System;

namespace SUM.CLI.UI
{
	public class InputHandler : IInputHandler
	{
		public InputCommand ReadCommandInput() =>
			CommandConverter.Convert(Console.ReadLine());

		public InputGate ReadGateInput() =>
			GateConverter.Convert(Console.ReadLine());
	}
}
