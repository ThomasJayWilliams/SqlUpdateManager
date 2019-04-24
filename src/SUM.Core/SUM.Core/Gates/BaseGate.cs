using SUM.Core.IO;

namespace SUM.Core
{
	public abstract class BaseGate : IGate
	{
		public static event InputGateHandler Input;
		public static event OutputHandler Output;

		public void OnOutput(string output) =>
			Output.Invoke(output);

		public GateActionDTO OnInput() =>
			Input.Invoke();
	}
}
