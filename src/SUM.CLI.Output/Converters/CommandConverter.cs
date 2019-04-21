using SUM.System;
using SUM.System.IO;

namespace SUM.CLI.UI.Converters
{
	public static class CommandConverter
	{
		public static ActionDTO Convert(string data)
		{
			var dto = new ActionDTO();

			switch (data)
			{
				case "exit":
				case "quit":
					dto.Action = InputAction.Exit;
					break;

				default:
					dto.Action = InputAction.Unknown;
					break;
			}

			return dto;
		}
	}
}
