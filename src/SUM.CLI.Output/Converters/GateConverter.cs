using SUM.System;

namespace SUM.CLI.UI.Converters
{
	public static class GateConverter
	{
		public static InputGate Convert(string data)
		{
			switch (data)
			{
				case "yes":
				case "y":
					return InputGate.Accept;

				case "no":
				case "n":
					return InputGate.Cancell;

				default:
					return InputGate.Unknown;
			}
		}
	}
}
