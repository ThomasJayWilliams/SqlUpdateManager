using SQLUpdateManager.CLI.Common;
using SQLUpdateManager.Core.Domains;
using System.Collections.Generic;
using SQLUpdateManager.CLI.IO;

namespace SQLUpdateManager.CLI.Application
{
	public class ConnectCommand : ICommand
	{
		public string Argument { get; set; }
        public bool HasParameters { get => true; }
        public bool RequiresArgument { get => false; }
		public IEnumerable<IParameter> Parameters { get; set; }
        public string Name { get => "connect"; }

        public void Execute()
		{
            var server = new DataServer();
            OutputHandler.Print("Server name: ");
            server.Name = InputHandler.ReadLine();

            if (string.IsNullOrWhiteSpace(server.Name))
                throw new InvalidCommandException(ErrorCodes.InvalidData, "The server name cannot be whitespace or empty!");

            OutputHandler.Print("Server address: ");
            server.Location = InputHandler.ReadLine();

            if (string.IsNullOrWhiteSpace(server.Location))
                throw new InvalidCommandException(ErrorCodes.InvalidData, "The server address cannot be whitespace or empty!");

            OutputHandler.Print("Server user name: ");
            server.Username = InputHandler.ReadLine();

            if (string.IsNullOrWhiteSpace(server.Username))
                throw new InvalidCommandException(ErrorCodes.InvalidData, "The server user name cannot be whitespace or empty!");

            OutputHandler.Print("Server user password: ");
            server.Username = InputHandler.ReadPassword();

            if (string.IsNullOrWhiteSpace(server.Username))
                throw new InvalidCommandException(ErrorCodes.InvalidData, "The server user password cannot be whitespace or empty!");

            OutputHandler.PrintLine("");

            Session.Current.ConnectedServer = server;
		}
	}
}
