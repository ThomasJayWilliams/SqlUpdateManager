using SQLUpdateManager.CLI.Common;
using SQLUpdateManager.CLI.IO;
using SQLUpdateManager.Core.Domains;
using SQLUpdateManager.Core.Registration;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SQLUpdateManager.CLI.Application
{
    public class RegisterCommand : BaseCommand
    {
        private readonly IDataRepository _repository;
        private readonly IOutput _output;
        private readonly Register _register;
        private readonly Session _session;
        private IParameter _listParameter
        {
            get => _parameters.FirstOrDefault(p => p.Name == CLIConstants.ListParameter);
        }

        protected override string[] AllowedParameters
        {
            get => new string[] { CLIConstants.ListParameter };
        }

        public override string Name { get => CLIConstants.RegisterCommand; }
        public override bool RequiresParameters { get => false; }
        public override bool RequiresArgument { get => true; }

        public RegisterCommand(
            Register register,
            Session session,
            IDataRepository repository,
            IOutput output)
        {
            _output = output;
            _register = register;
            _session = session;
            _repository = repository;
        }

        public override void Execute()
        {
            if (_listParameter != null)
            {
                var theme = _session.Theme;
                var servers = _register.GetAll();

                if (servers != null && servers.Any())
                {
                    foreach (var server in servers)
                    {
                        _output.PrintColoredLine(server.ToString(), theme.ServerColor);

                        if (server.Databases != null && server.Databases.Any())
                        {
                            foreach (var database in server.Databases)
                            {
                                _output.PrintColoredLine($"\t{database.ToString()}", theme.DatabaseColor);

                                if (database.Procedures != null && database.Procedures.Any())
                                {
                                    foreach (var procedure in database.Procedures)
                                        _output.PrintColoredLine($"\t\t{procedure.ToString()}", theme.ProcedureColor);
                                }
                            }
                        }
                    }
                }

                else
                    _output.PrintColoredLine("Currently registry is empty.", _session.Theme.TextColor);
            }

            else
            {
                if (string.IsNullOrEmpty(Argument))
                    throw new InvalidArgumentException(ErrorCodes.CommandRequiresArgument, $"{Name} command requires argument.");

                var procedure = _repository.GetRawData(Argument);
                var procName = Path.GetFileName(Argument);

                if (string.IsNullOrEmpty(procedure))
                    throw new InvalidArgumentException(ErrorCodes.InvalidArgument, $"{Argument} procedure contains no data. Cannot register empty files.");

                var server = _session.ConnectedServer;
                var database = _session.UsedDatabase;

                if (server == null)
                    throw new InvalidStateException(ErrorCodes.ServerIsNotConnected, "You are not connected to any server.");
                if (database == null)
                    throw new InvalidStateException(ErrorCodes.NoDatabaseInUse, "You are not using any database.");

                var isServerExist = _register.IsExist(server.Hash);

                if (!isServerExist)
                {
                    var newServer = new Server
                    {
                        Name = server.Name,
                        Type = server.Type,
                        Databases = new List<Database>
                        {
                            new Database
                            {
                                Name = _session.UsedDatabase.Name,
                                Procedures = new List<Procedure>
                                {
                                    new Procedure { Name = procName, Location = Argument }
                                }
                            }
                        }
                    };

                    _register.AddServer(newServer);
                    _register.SaveChanges();
                }

                else if (isServerExist)
                {
                    var existingServer = _register.GetServer(server.Hash);
                    var existingDatabase = existingServer.Databases.FirstOrDefault(db => db.Hash.SequenceEqual(database.Hash));

                    if (existingDatabase != null)
                    {
                        var existingProcedure = existingDatabase.Procedures.FirstOrDefault(proc => proc.Name == procName);

                        if (existingProcedure != null)
                            throw new InvalidStateException(ErrorCodes.ProcedureIsAlreadyRegistered,
                                $"{procName} procedure is already registered and being tracked.");
                        else
                        {
                            var procedures = existingDatabase.Procedures.ToList();
                            procedures.Add(new Procedure { Name = procName, Location = Argument });
                            existingDatabase.Procedures = procedures;
                        }
                    }

                    else
                    {
                        var databases = existingServer.Databases.ToList();
                        databases.Add(new Database
                        {
                            Name = database.Name,
                            Procedures = new List<Procedure> { new Procedure { Name = procName, Location = Argument } }
                        });
                        existingServer.Databases = databases;
                    }

                    _register.SaveChanges();
                }
            }
        }
    }
}
