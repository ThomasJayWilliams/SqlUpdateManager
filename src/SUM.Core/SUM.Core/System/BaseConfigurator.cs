using SUM.Core.IO;
using SUM.Core.Managers;

namespace SUM.Core
{
	public static class BaseConfigurator
	{
		public static void RegisterGateEvents(InputGateHandler input, OutputHandler output)
		{
			BaseGate.Input += input;
			BaseGate.Output += output;
		}

		public static void RegisterManagersEvents(OutputHandler output)
		{
			BaseManager.Output += output;
		}
	}
}
