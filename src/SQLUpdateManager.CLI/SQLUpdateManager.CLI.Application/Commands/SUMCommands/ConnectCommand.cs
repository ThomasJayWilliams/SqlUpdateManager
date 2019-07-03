using System.Collections.Generic;

namespace SQLUpdateManager.CLI.Application
{
	public class ConnectCommand : ISUMCommand
	{
		public IArgument Argument { get; set; }
		public IEnumerable<IParameter> Parameters { get; set; }

		public void Execute()
		{
			
		}
	}
}
