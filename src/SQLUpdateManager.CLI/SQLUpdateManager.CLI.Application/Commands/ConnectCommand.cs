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
            get => _parameters.FirstOrDefault(p => p.Name == CLIConstants.SaveParameter);
        }

        private readonly Session _session;
        private readonly IOutput _output;
        private readonly IInput _input;

        protected override string[] AllowedParameters
        {
            get => new string[]
            {
                CLIConstants.SaveParameter
            };
        }

        public override bool RequiresParameters { get => false; }
        public override bool RequiresArgument { get => false; }
        public override string Name { get => CLIConstants.ConnectCommand; }

        public ConnectCommand(Session session, IOutput output, IInput input)
        {
            _input = input;
            _output = output;
            _session = session;
        }

        public override void Execute()
        {
            if (!string.IsNullOrEmpty(Argument))
                throw new InvalidCommandException(ErrorCodes.MissplacedArgument, $"{Name} does not expect argument.");

            if (_session.ConnectedServer != null)
            {
                _output.PrintColored(
                    $"You are currently connected to the {_session.ConnectedServer.Name} server." +
                    $"Before execution of this command you will be disconnected from this server. Continue?(y/n)",
                    _session.Theme.TextColor);

                if (_input.ReadLine() != "y")
                    throw new InvalidCommandException(ErrorCodes.InvalidCommand, "Invalid input. Aborting.");

                _session.ConnectedServer = null;
                _session.UsedDatabase = null;
            }

            Connect();
        }

        private void Connect()
        {
            var server = new DataServer();
            _output.PrintColored("Server name: ", _session.Theme.TextColor);
            server.Name = _input.ReadLine();

            if (string.IsNullOrWhiteSpace(server.Name))
                throw new InvalidCommandException(ErrorCodes.InvalidData, "The server name cannot be whitespace or empty!");

            _output.PrintColored("Server address: ", _session.Theme.TextColor);
            server.Location = _input.ReadLine();

            if (string.IsNullOrWhiteSpace(server.Location))
                throw new InvalidCommandException(ErrorCodes.InvalidData, "The server address cannot be whitespace or empty!");

            _output.PrintColored("Server user name: ", _session.Theme.TextColor);
            server.Username = _input.ReadLine();

            if (string.IsNullOrWhiteSpace(server.Username))
                throw new InvalidCommandException(ErrorCodes.InvalidData, "The server user name cannot be whitespace or empty!");

            _output.PrintColored("Server user password: ", _session.Theme.TextColor);
            server.Username = _input.ReadPassword();

            if (string.IsNullOrWhiteSpace(server.Username))
                throw new InvalidCommandException(ErrorCodes.InvalidData, "The server user password cannot be whitespace or empty!");

            _session.ConnectedServer = server;
        }
    }
}
