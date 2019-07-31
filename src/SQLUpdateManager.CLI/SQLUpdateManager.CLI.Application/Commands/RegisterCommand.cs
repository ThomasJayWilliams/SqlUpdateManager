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
        private IParameter _deleteParameter
        {
            get => _parameters.FirstOrDefault(p => p.Name == CLIConstants.DeleteParameter);
        }

        protected override string[] AllowedParameters
        {
            get => new string[]
            {
                CLIConstants.ListParameter,
                CLIConstants.DeleteParameter
            };
        }

        public override string Name { get => CLIConstants.RegisterCommand; }

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

        protected override void Validation()
        {
            if (_listParameter != null && _deleteParameter != null)
                throw new InvalidParameterException(ErrorCodes.ConflictParameters,
                    $"The {_listParameter.Name} and {_deleteParameter.Name} parameters cannot be used at the same time.");

            if (!string.IsNullOrEmpty(Argument) && _listParameter != null || _deleteParameter != null)
                throw new InvalidArgumentException(ErrorCodes.MissplacedArgument,
                    $"The {_listParameter.Name} and {_deleteParameter.Name} parameters exclude argument.");

            else if (string.IsNullOrEmpty(Argument))
                throw new InvalidArgumentException(ErrorCodes.CommandRequiresArgument, $"{Name} command requires argument.");
        }

        protected override void Execute()
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

                        if (server.Databases != null)
                        {
                            foreach (var database in server.Databases)
                            {
                                _output.PrintColoredLine($"\t{database.ToString()}", theme.DatabaseColor);

                                if (database.Procedures != null)
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

            else if (_deleteParameter != null)
            {
                if (_session.ConnectedServer == null)
                {
                    if (Argument == "*")
                    {
                        var servers = _register.GetAll();

                        if (!servers.Any())
                            _output.PrintColoredLine("No registered servers.", _session.Theme.TextColor);

                        else
                        {
                            foreach (var server in servers)
                            {
                                _output.PrintColored($"Removing ", _session.Theme.TextColor);
                                _output.PrintColored(server.Name, _session.Theme.ServerColor);
                                _output.PrintColoredLine("...", _session.Theme.TextColor);
                                _register.RemoveServer(server.Hash);
                            }
                        }
                    }

                    else
                    {
                        var server = _register.GetServerByName(Argument);

                        if (server == null)
                            throw new InvalidArgumentException(ErrorCodes.ServerIsNotRegistered,
                                $"{Argument} server is not registered.");

                        _output.PrintColored($"Removing ", _session.Theme.TextColor);
                        _output.PrintColored(server.Name, _session.Theme.ServerColor);
                        _output.PrintColoredLine("...", _session.Theme.TextColor);

                        _register.RemoveServer(server.Hash);
                    }

                    _register.SaveChanges();
                }

                else if (_session.ConnectedServer != null && _session.UsedDatabase == null)
                {
                    var server = _register.GetServer(_session.ConnectedServer.Hash);

                    if (server == null)
                        throw new InvalidArgumentException(ErrorCodes.ServerIsNotRegistered,
                            $"{_session.ConnectedServer.Name} server is not registered and cannot be removed.");

                    if (server.Databases != null)
                    {
                        if (Argument == "*")
                        {
                            if (!server.Databases.Any())
                                _output.PrintColoredLine("No registered databases.", _session.Theme.TextColor);

                            foreach (var database in server.Databases)
                            {
                                _output.PrintColored($"Removing ", _session.Theme.TextColor);
                                _output.PrintColored(database.Name, _session.Theme.DatabaseColor);
                                _output.PrintColoredLine("...", _session.Theme.TextColor);
                            }

                            server.Databases = null;
                        }

                        else
                        {
                            var database = server.Databases.FirstOrDefault(db => db.Name == Argument);

                            if (database == null)
                                throw new InvalidArgumentException(ErrorCodes.DatabaseIsNotRegistered,
                                    $"{Argument} database is not registered.");

                            _output.PrintColored($"Removing ", _session.Theme.TextColor);
                            _output.PrintColored(database.Name, _session.Theme.DatabaseColor);
                            _output.PrintColoredLine("...", _session.Theme.TextColor);

                            server.Databases = server.Databases.Where(db => !db.Hash.SequenceEqual(database.Hash));
                        }
                    }

                    _register.UpdateServer(server);
                    _register.SaveChanges();
                }

                else if (_session.UsedDatabase != null)
                {
                    var server = _register.GetServer(_session.ConnectedServer.Hash);

                    if (server == null)
                        throw new InvalidArgumentException(ErrorCodes.ServerIsNotRegistered,
                            $"{_session.ConnectedServer.Name} server is not registered and cannot be removed.");

                    if (server.Databases == null || !server.Databases.Any(db => db.Hash.SequenceEqual(_session.UsedDatabase.Hash)))
                        throw new InvalidArgumentException(ErrorCodes.DatabaseIsNotRegistered,
                            $"{_session.UsedDatabase.Name} database is not registered and cannot be removed.");

                    var database = server.Databases.FirstOrDefault(db => db.Hash.SequenceEqual(_session.UsedDatabase.Hash));

                    if (database.Procedures != null)
                    {
                        if (Argument == "*")
                        {
                            if (!database.Procedures.Any())
                                _output.PrintColoredLine("No registered procedures.", _session.Theme.TextColor);

                            foreach (var procedure in database.Procedures)
                            {
                                _output.PrintColored($"Removing ", _session.Theme.TextColor);
                                _output.PrintColored(procedure.Name, _session.Theme.ProcedureColor);
                                _output.PrintColoredLine("...", _session.Theme.TextColor);
                            }

                            database.Procedures = null;
                        }

                        else
                        {
                            var procedure = database.Procedures.FirstOrDefault(proc => proc.Name == Argument);

                            if (procedure == null)
                                throw new InvalidArgumentException(ErrorCodes.ProcedureIsNotRegistered,
                                    $"{Argument} procedure is not registered.");

                            _output.PrintColored($"Removing ", _session.Theme.TextColor);
                            _output.PrintColored(procedure.Name, _session.Theme.ProcedureColor);
                            _output.PrintColoredLine("...", _session.Theme.TextColor);

                            database.Procedures = database.Procedures.Where(proc => !proc.Hash.SequenceEqual(procedure.Hash));
                        }
                    }

                    _register.UpdateServer(server);
                    _register.SaveChanges();
                }
            }

            else
            {
                var procName = Path.GetFileName(Argument);
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
