using SQLUpdateManager.CLI.Common;
using SQLUpdateManager.CLI.IO;
using SQLUpdateManager.Core.Domains;

namespace SQLUpdateManager.CLI.Application
{
    public class ConnectCommand : BaseCommand
    {
        public override bool RequiresParameters { get => false; }
        public override bool RequiresArgument { get => false; }
        public override string Name { get => Constants.ConnectCommand; }

        public override void Execute()
        {
            if (Session.Current.ConnectedServer != null)
            {
                OutputHandler.Print(
                    $"You are currently connected to the {Session.Current.ConnectedServer.Name} server." +
                    $"Before execution of this command you will be disconnected from this server. Continue?(y/n)");

                if (InputHandler.ReadLine() != "y")
                    throw new InvalidCommandException(ErrorCodes.InvalidCommand, "Invalid input. Aborting.");

                Session.Current.ConnectedServer = null;
            }

            Connect();
        }

        private void Connect()
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
