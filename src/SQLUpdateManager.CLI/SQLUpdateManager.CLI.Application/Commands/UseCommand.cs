using SQLUpdateManager.CLI.Common;
using SQLUpdateManager.CLI.IO;
using SQLUpdateManager.Core.Domains;
using System;
using System.Collections.Generic;
using System.Text;

namespace SQLUpdateManager.CLI.Application
{
    public class UseCommand : ICommand
    {
        public string Name { get => "use"; }

        public IArgument Argument { get; set; }
        public IEnumerable<IParameter> Parameters { get; set; }

        public bool HasArgument { get => false; }

        public void Execute()
        {
            if (Session.Current.ConnectedServer == null)
                throw new InvalidStateException(ErrorCodes.ServerIsNotConnected, "To use database you must be connected to the server! Use 'connect --help' to get help.");

            var db = new DataDatabase();

            OutputHandler.Print("Database name: ");
            db.Name = InputHandler.ReadLine();

            if (string.IsNullOrWhiteSpace(db.Name))
                throw new InvalidCommandException(ErrorCodes.InvalidData, "The database name cannot be whitespace or empty!");

            Session.Current.UsedDatabase = db;
        }
    }
}
