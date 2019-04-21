using SUM.System;

namespace SUM.CLI.UI.Converters
{
	public static class CommandConverter
	{
		public static InputCommand Convert(string data)
		{
			switch (data)
			{
				case "exit":
				case "quit":
					return InputCommand.Exit;

				default:
					return InputCommand.Unknown;
			}
		}
	}
}
