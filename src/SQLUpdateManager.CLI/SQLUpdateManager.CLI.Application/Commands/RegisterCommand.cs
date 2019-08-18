using SqlUpdateManager.CLI.Common;
using SqlUpdateManager.CLI.IO;
using SqlUpdateManager.Core.Common;
using SqlUpdateManager.Core.Registry;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SqlUpdateManager.CLI.Application
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
                    $"The {CLIConstants.ListParameter} and {CLIConstants.DeleteParameter} parameters cannot be used at the same time.");

            if (!string.IsNullOrEmpty(Argument) && _listParameter != null)
                throw new InvalidArgumentException(ErrorCodes.MissplacedArgument,
                    $"The {CLIConstants.ListParameter} parameter excludes argument.");

            if (string.IsNullOrEmpty(Argument) && _deleteParameter != null)
                throw new InvalidArgumentException(ErrorCodes.CommandRequiresArgument,
                    $"The {CLIConstants.DeleteParameter} parameters requires command argument to be specified.");

            else if (string.IsNullOrEmpty(Argument) && _listParameter == null && _deleteParameter == null)
                throw new InvalidArgumentException(ErrorCodes.CommandRequiresArgument, $"{Name} command requires argument.");
        }

        protected override void Execute()
        {
            if (_listParameter != null)
            {
                var servers = _register.GetAll();

                if (servers != null && servers.Any())
                {
                    foreach (var server in servers)
                    {
                        _output.PrintLine(server.ToString());

                        if (server.Databases != null)
                        {
                            foreach (var database in server.Databases)
                            {
                                _output.PrintLine($"\t{database.ToString()}");

                                if (database.Procedures != null)
                                {
                                    foreach (var procedure in database.Procedures)
                                        _output.PrintLine($"\t\t{procedure.ToString()}");
                                }
                            }
                        }
                    }
                }

                else
                    _output.PrintLine("Currently registry is empty.");
            }

            else if (_deleteParameter != null)
            {
                if (_session.ConnectedServer == null)
                {
                    if (Argument == "*")
                    {
                        var servers = _register.GetAll();

                        if (!servers.Any())
                            _output.PrintLine("No registered servers.");

                        else
                        {
                            foreach (var server in servers)
                            {
                                _output.Print($"Removing ");
                                _output.Print(server.Name);
                                _output.PrintLine("...");
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

                        _output.Print($"Removing ");
                        _output.Print(server.Name);
                        _output.PrintLine("...");

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
                                _output.PrintLine("No registered databases.");

                            foreach (var database in server.Databases)
                            {
                                _output.Print($"Removing ");
                                _output.Print(database.Name);
                                _output.PrintLine("...");
                            }

                            server.Databases = null;
                        }

                        else
                        {
                            var database = server.Databases.FirstOrDefault(db => db.Name == Argument);

                            if (database == null)
                                throw new InvalidArgumentException(ErrorCodes.DatabaseIsNotRegistered,
                                    $"{Argument} database is not registered.");

                            _output.Print($"Removing ");
                            _output.Print(database.Name);
                            _output.PrintLine("...");

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
                                _output.PrintLine("No registered procedures.");

                            foreach (var procedure in database.Procedures)
                            {
                                _output.Print($"Removing ");
                                _output.Print(procedure.Name);
                                _output.PrintLine("...");
                            }

                            database.Procedures = null;
                        }

                        else
                        {
                            var procedure = database.Procedures.FirstOrDefault(proc => proc.Name == Argument);

                            if (procedure == null)
                                throw new InvalidArgumentException(ErrorCodes.ProcedureIsNotRegistered,
                                    $"{Argument} procedure is not registered.");

                            _output.Print($"Removing ");
                            _output.Print(procedure.Name);
                            _output.PrintLine("...");

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
