using SUM.Core.IO;

namespace SUM.Core
{
	public class BinaryGate : BaseGate, IBinaryGate
	{
		public bool Request(string output)
		{
			OnOutput(output);

			if (OnInput().Action == InputGate.Accept)
				return true;

			return false;
		}
	}
}
