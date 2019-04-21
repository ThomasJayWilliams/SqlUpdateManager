using SUM.System.IO;
using System;

namespace SUM.CLI.UI
{
	public class OutputHandler : IOutputHandler
	{
		public void Out(string data) =>
			Console.WriteLine(data);
	}
}
