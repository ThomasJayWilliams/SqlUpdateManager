using SQLUpdateManager.CLI.Common;
using SQLUpdateManager.CLI.IO;
using SQLUpdateManager.Core.Domains;
using System.Collections.Generic;
using System.Linq;

namespace SQLUpdateManager.CLI.Application
{
    public class ConnectCommand : BaseCommand
    {
        private IParameter _saveParameter
        {
            get => _parameters.FirstOrDefault(p => p.Name == CLIConstants.SaveParameter);
        }

        private readonly IInput _input;
        private readonly IOutput _output;
        private readonly Session _session;
        private readonly IDataRepository _dataRepo;

        protected override string[] AllowedParameters
        {
            get => new string[]
            {
                CLIConstants.SaveParameter
            };
        }

        public override string Name { get => CLIConstants.ConnectCommand; }

        public ConnectCommand(
            IInput input,
            IOutput output,
            Session session,
            IDataRepository dataRepo)
        {
            _input = input;
            _output = output;
            _session = session;
            _dataRepo = dataRepo;
        }

        protected override void Validation()
        {
            if (_session.ConnectedServer != null && Argument != "/")
                throw new InvalidStateException(ErrorCodes.AlreadyConnectedToTheServer,
                    $"Cannot connect. Already connected to {_session.ConnectedServer.Name}.");

            if (_saveParameter != null)
            {
                if (!string.IsNullOrEmpty(Argument))
                    throw new InvalidArgumentException(ErrorCodes.MissplacedArgument,
                        $"The {_saveParameter.Name} parameter excludes accepting argument.");

                if (string.IsNullOrWhiteSpace(_saveParameter.Argument))
                    throw new InvalidArgumentException(ErrorCodes.InvalidArgument,
                        $"Invalid argument for {_saveParameter.Name} parameter.");
            }
        }

        protected override void Execute()
        {
            var server = new DataServer();

            if (!string.IsNullOrEmpty(Argument))
            {
                if (Argument == "/")
                {
                    _session.ConnectedServer = null;
                    _session.UsedDatabase = null;
                }

                else
                {
                    var storageServer = _session.Storage.Servers.FirstOrDefault(s => s.Alias == Argument);

                    if (storageServer == null)
                        throw new InvalidArgumentException(ErrorCodes.ServerIsNotStored,
                            $"There is no server with {Argument} alias in storage.");

                    server.Name = storageServer.Name;
                    server.Location = storageServer.Location;
                    server.Username = storageServer.Username;

                    _output.Print("Server user password: ");
                    server.Password = _input.ReadPassword();

                    if (string.IsNullOrWhiteSpace(server.Password))
                        throw new InvalidCommandException(ErrorCodes.InvalidData, "The server user password cannot be whitespace or empty!");

                    _session.ConnectedServer = server;
                }
            }

            else
            {
                _output.Print("Server name: ");
                server.Name = _input.ReadLine();

                if (string.IsNullOrWhiteSpace(server.Name))
                    throw new InvalidCommandException(ErrorCodes.InvalidData, "The server name cannot be whitespace or empty!");

                _output.Print("Server address: ");
                server.Location = _input.ReadLine();

                if (string.IsNullOrWhiteSpace(server.Location))
                    throw new InvalidCommandException(ErrorCodes.InvalidData, "The server address cannot be whitespace or empty!");

                _output.Print("Server user name: ");
                server.Username = _input.ReadLine();

                if (string.IsNullOrWhiteSpace(server.Username))
                    throw new InvalidCommandException(ErrorCodes.InvalidData, "The server user name cannot be whitespace or empty!");

                _output.Print("Server user password: ");
                server.Password = _input.ReadPassword();

                if (string.IsNullOrWhiteSpace(server.Password))
                    throw new InvalidCommandException(ErrorCodes.InvalidData, "The server user password cannot be whitespace or empty!");

                _session.ConnectedServer = server;
            }

            if (_saveParameter != null)
            {
                var storageServer = new StorageServer
                {
                    Alias = _saveParameter.Argument,
                    Name = server.Name,
                    Location = server.Location,
                    Username = server.Username
                };

                if (_session.Storage == null)
                    throw new InvalidStateException(ErrorCodes.CannotReadStorage, "Error occurred while reading the storage.");

                if (_session.Storage.Servers != null && _session.Storage.Servers.Any())
                {
                    if (_session.Storage.Servers.Any(s => s.Alias == _saveParameter.Argument))
                        throw new InvalidArgumentException(ErrorCodes.ServerIsAlreadyInStorage,
                            $"{_saveParameter.Argument} server is already saved into the storage.");

                    var servers = _session.Storage.Servers.ToList();

                    servers.Add(storageServer);
                    _session.Storage.Servers = servers;
                }

                else
                    _session.Storage.Servers = new List<StorageServer>
                    {
                        storageServer
                    };

                _dataRepo.WriteData(CLIConstants.StoragePath, _session.Storage);
            }
        }
    }
}
