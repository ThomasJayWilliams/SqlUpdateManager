using SUM.Core;
using SUM.Core.IO;

namespace SUM.CLI
{
	public static class GateConverter
	{
		public static GateActionDTO Convert(string data)
		{
			var dto = new GateActionDTO();

			switch (data)
			{
				case "yes":
				case "y":
					dto.Action = InputGate.Accept;
					break;

				case "no":
				case "n":
					dto.Action = InputGate.Cancell;
					break;

				default:
					dto.Action = InputGate.Unknown;
					break;
			}

			return dto;
		}
	}
}
