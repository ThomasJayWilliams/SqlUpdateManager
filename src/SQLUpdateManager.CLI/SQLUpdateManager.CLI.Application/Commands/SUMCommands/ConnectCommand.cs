using SQLUpdateManager.CLI.Common;
using SQLUpdateManager.Core.Domains;
using System.Collections.Generic;

namespace SQLUpdateManager.CLI.Application
{
	public class ConnectCommand : ISUMCommand
	{
		public IArgument Argument { get; set; }
		public IEnumerable<IParameter> Parameters { get; set; }
        public string Name { get => "sum connect"; }

        public void Execute()
		{
            Session.Current.ConnectedServer = new Server
            {
                Name = Argument.Value,
                Type = ServerType.MySql
            };
		}
	}
}
