using SUM.Core.IO;
using System;

namespace SUM.CLI.UI
{
	public static class InputHandler
	{
		public static ActionDTO ReadCommandInput() =>
			CommandConverter.Convert(Console.ReadLine());

		public static GateActionDTO ReadGateInput() =>
			GateConverter.Convert(Console.ReadLine());
	}
}
