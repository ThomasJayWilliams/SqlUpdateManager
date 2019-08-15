﻿using SQLUpdateManager.CLI.Common;
using SQLUpdateManager.CLI.IO;
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

        protected override void Validation()
        {
            if (!string.IsNullOrEmpty(Argument))
                throw new InvalidArgumentException(ErrorCodes.MissplacedArgument,
                    $"The {Name} command does not accept arguments.");

            if (_listParameter != null && _deleteParameter != null)
                throw new InvalidParameterException(ErrorCodes.ConflictParameters,
                    $"The {_listParameter.Name} and {_deleteParameter.Name} parameters cannot be used at the same time.");
        }

        protected override void Execute()
        {
            if (_listParameter != null)
            {
                var servers = _session.Storage.Servers;

                if (servers != null && servers.Any())
                {
                    _output.PrintLine("Stored servers:");

					foreach (var server in servers)
                    {
                        _output.Print($"{server.Alias}: ");
						_output.PrintLine($"{server.Name} {server.Location} {server.Username}");
					}
                }

                else
                    _output.PrintLine("Storage is empty.");
			}

            else if (_deleteParameter != null)
            {
                _session.Storage.Servers = Enumerable.Empty<StorageServer>();
                _dataRepo.WriteData(CLIConstants.StoragePath, _session.Storage);

                _output.PrintLine("Storage is cleared.");
			}
        }
    }
}