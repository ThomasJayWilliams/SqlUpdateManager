using SQLUpdateManager.CLI.Common;
using SQLUpdateManager.Core.Domains;
using System.Collections.Generic;

namespace SQLUpdateManager.CLI.Application
{
    public class UseCommand : ICommand
    {
        public string Name { get => Constants.UseCommand; }
        public string Argument { get; set; }
        public bool HasParameters { get => true; }
        public bool RequiresArgument { get => true; }
        public IEnumerable<IParameter> Parameters { get; set; }

        public void Execute()
        {
            if (Session.Current.ConnectedServer == null)
                throw new InvalidStateException(ErrorCodes.ServerIsNotConnected, "To use database you must be connected to the server! Use 'connect --help' to get help.");
            if (string.IsNullOrEmpty(Argument))
                throw new InvalidCommandException(ErrorCodes.InvalidArgument, $"{Name} command requires database name as an argument.");

            var db = new DataDatabase { Name = Argument };

            if (string.IsNullOrWhiteSpace(db.Name))
                throw new InvalidCommandException(ErrorCodes.InvalidData, "The database name cannot be whitespace or empty!");

            Session.Current.UsedDatabase = db;
        }
    }
}
