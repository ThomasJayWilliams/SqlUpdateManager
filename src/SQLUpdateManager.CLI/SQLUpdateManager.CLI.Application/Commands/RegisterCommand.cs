using SQLUpdateManager.CLI.Common;
using SQLUpdateManager.CLI.IO;
using SQLUpdateManager.Core.Domains;
using SQLUpdateManager.Core.Registration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SQLUpdateManager.CLI.Application
{
    public class RegisterCommand : BaseCommand
    {
        private ListParameter _listParameter;
        private readonly IDataRepository _repository;
        private readonly Register _register;

        public override string Name { get => Constants.RegisterCommand; }
        public override bool RequiresParameters { get => false; }
        public override bool RequiresArgument { get => true; }

        public RegisterCommand(Register register, IDataRepository repository)
        {
            _register = register;
            _repository = repository;
        }

        public override void AddParameters(params IParameter[] parameters)
        {
            if (parameters == null || !parameters.Any())
                throw new ArgumentNullException("Parameters cannot be null or empty!");

            foreach (var param in parameters)
            {
                if (param.Name == Constants.ListParameter)
                    _listParameter = param as ListParameter;
                else
                    throw new InvalidParameterException(ErrorCodes.UnacceptableParameter, $"{Name} command does not accept {param.Name} parameter.");
            }

            _parameters.AddRange(parameters);
        }

        public override void Execute()
        {
            if (_listParameter != null)
            {
                var servers = _register.GetAll();

                if (servers != null && servers.Any())
                {
                    foreach (var server in servers)
                    {
                        Output.PrintLine($"Server: {server.Name}");

                        if (server.Databases != null && server.Databases.Any())
                        {
                            Output.PrintEmptyLine();
                            Output.PrintLine("Databases: ");

                            foreach (var database in server.Databases)
                            {
                                Output.PrintLine($"\t{database.Name}");

                                if (database.Procedures != null && database.Procedures.Any())
                                {
                                    Output.PrintEmptyLine();
                                    Output.PrintLine($"\tProcedures: ");

                                    foreach (var procedure in database.Procedures)
                                        Output.PrintLine($"\t\t{procedure.Name}");
                                }
                            }
                        }
                    }
                }
            }

            else
            {
                if (string.IsNullOrEmpty(Argument))
                    throw new InvalidArgumentException(ErrorCodes.CommandRequiresArgument, $"{Name} command requires argument.");

                var procedure = _repository.GetRawData(Argument);
                var procName = Path.GetFileName(Argument);

                if (string.IsNullOrEmpty(procedure))
                    throw new InvalidArgumentException(ErrorCodes.InvalidArgument, $"{Argument} procedure contains no data. Cannot register empty files.");

                var server = Session.Current.ConnectedServer;
                var database = Session.Current.UsedDatabase;

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
                                Name = Session.Current.UsedDatabase.Name,
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
