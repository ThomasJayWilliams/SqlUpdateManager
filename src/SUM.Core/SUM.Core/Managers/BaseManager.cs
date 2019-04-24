using SUM.Core.IO;

namespace SUM.Core.Managers
{
	public class BaseManager
	{
		public static event OutputHandler Output;

		public void OnOutput(string output) =>
			Output.Invoke(output);
	}
}
