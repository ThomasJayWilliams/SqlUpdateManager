using SQLUpdateManager.CLI.Common;
using SQLUpdateManager.CLI.IO;
using System;
using System.Drawing;
using System.Linq;

namespace SQLUpdateManager.CLI.Application
{
    public class StorageCommand : BaseCommand
    {
        private IParameter _listParameter
        {
            get => _parameters.FirstOrDefault(p => p.Name == CLIConstants.ListParameter);
        }
        private IParameter _deleteParameter
        {
            get => _parameters.FirstOrDefault(p => p.Name == CLIConstants.DeleteParameter);
        }

        private readonly IOutput _output;
        private readonly Session _session;
        private readonly IDataRepository _dataRepo;

        public override string Name { get => CLIConstants.StorageCommand; }
        public override bool RequiresParameters { get => true; }
        public override bool RequiresArgument { get => false; }

        public StorageCommand(Session session, IDataRepository dataRepo, IOutput output)
        {
            _output = output;
            _session = session;
            _dataRepo = dataRepo;
        }

        protected override string[] AllowedParameters
        {
            get => new string[]
            {
                CLIConstants.ListParameter,
                CLIConstants.DeleteParameter
            };
        }

        public override void Execute()
        {
            if (!string.IsNullOrEmpty(Argument))
                throw new InvalidArgumentException(ErrorCodes.MissplacedArgument,
                    $"The {Name} command does not accept arguments.");

            if (_listParameter != null)
            {
                if (_deleteParameter != null)
                    throw new InvalidParameterException(ErrorCodes.ConflictParameters,
                        $"The {_listParameter.Name} and {_deleteParameter.Name} parameters cannot be used at the same time.");

                var servers = _session.Storage.Servers;

                if (servers != null && servers.Any())
                {
                    _output.PrintColoredLine("Stored servers:", _session.Theme.TextColor);

                    foreach (var server in servers)
                    {
                        _output.PrintColored($"{server.Alias}: ", _session.Theme.PropertyColor);
                        _output.PrintColoredLine($"{server.Name} {server.Location} {server.Username}", _session.Theme.TextColor);
                    }
                }

                else
                    _output.PrintColoredLine("Storage is empty.", _session.Theme.TextColor);
            }

            else if (_deleteParameter != null)
            {
                if (_listParameter != null)
                    throw new InvalidParameterException(ErrorCodes.ConflictParameters,
                        $"The {_listParameter.Name} and {_deleteParameter.Name} parameters cannot be used at the same time.");

                _session.Storage.Servers = Enumerable.Empty<StorageServer>();
                _dataRepo.WriteData(CLIConstants.StoragePath, _session.Storage);

                _output.PrintColoredLine("Storage is cleared.", _session.Theme.TextColor);
            }
        }
    }
}
