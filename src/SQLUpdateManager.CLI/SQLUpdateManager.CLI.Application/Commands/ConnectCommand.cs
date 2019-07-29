using SQLUpdateManager.CLI.Common;
using SQLUpdateManager.CLI.IO;
using SQLUpdateManager.Core.Domains;
using System.Linq;

namespace SQLUpdateManager.CLI.Application
{
    public class ConnectCommand : BaseCommand
    {
        private IParameter _saveParameter
        {
            get => _parameters.FirstOrDefault(p => p.Name == Constants.SaveParameter);
        }

        protected override string[] AllowedParameters
        {
            get => new string[]
            {
                Constants.SaveParameter
            };
        }

        public override bool RequiresParameters { get => false; }
        public override bool RequiresArgument { get => false; }
        public override string Name { get => Constants.ConnectCommand; }

        public override void Execute()
        {
            if (!string.IsNullOrEmpty(Argument))
                throw new InvalidCommandException(ErrorCodes.MissplacedArgument, $"{Name} does not expect argument.");

            if (Session.Current.ConnectedServer != null)
            {
                Output.Print(
                    $"You are currently connected to the {Session.Current.ConnectedServer.Name} server." +
                    $"Before execution of this command you will be disconnected from this server. Continue?(y/n)");

                if (Input.ReadLine() != "y")
                    throw new InvalidCommandException(ErrorCodes.InvalidCommand, "Invalid input. Aborting.");

                Session.Current.ConnectedServer = null;
                Session.Current.UsedDatabase = null;
            }

            Connect();
        }

        private void Connect()
        {
            var server = new DataServer();
            Output.Print("Server name: ");
            server.Name = Input.ReadLine();

            if (string.IsNullOrWhiteSpace(server.Name))
                throw new InvalidCommandException(ErrorCodes.InvalidData, "The server name cannot be whitespace or empty!");

            Output.Print("Server address: ");
            server.Location = Input.ReadLine();

            if (string.IsNullOrWhiteSpace(server.Location))
                throw new InvalidCommandException(ErrorCodes.InvalidData, "The server address cannot be whitespace or empty!");

            Output.Print("Server user name: ");
            server.Username = Input.ReadLine();

            if (string.IsNullOrWhiteSpace(server.Username))
                throw new InvalidCommandException(ErrorCodes.InvalidData, "The server user name cannot be whitespace or empty!");

            Output.Print("Server user password: ");
            server.Username = Input.ReadPassword();

            if (string.IsNullOrWhiteSpace(server.Username))
                throw new InvalidCommandException(ErrorCodes.InvalidData, "The server user password cannot be whitespace or empty!");

            Output.PrintLine("");

            Session.Current.ConnectedServer = server;
        }
    }
}
