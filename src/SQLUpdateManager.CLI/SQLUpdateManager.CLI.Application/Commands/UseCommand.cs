using SQLUpdateManager.CLI.Common;
using SQLUpdateManager.Core.Domains;

namespace SQLUpdateManager.CLI.Application
{
    public class UseCommand : BaseCommand
    {
        private readonly Session _session;

        protected override string[] AllowedParameters
        {
            get => new string[] { };
        }

        public override string Name { get => CLIConstants.UseCommand; }
        public override bool RequiresParameters { get => false; }
        public override bool RequiresArgument { get => true; }

        public UseCommand(Session session)
        {
            _session = session;
        }

        public override void Execute()
        {
            if (_session.ConnectedServer == null)
                throw new InvalidStateException(ErrorCodes.ServerIsNotConnected, "To use database you must be connected to the server! Use 'connect --help' to get help.");
            if (string.IsNullOrEmpty(Argument))
                throw new InvalidCommandException(ErrorCodes.InvalidArgument, $"{Name} command requires database name as an argument.");

            if (Argument == "/")
                _session.UsedDatabase = null;

            else
            {
                var db = new DataDatabase { Name = Argument };

                if (string.IsNullOrWhiteSpace(db.Name))
                    throw new InvalidCommandException(ErrorCodes.InvalidData, "The database name cannot be whitespace or empty!");

                _session.UsedDatabase = db;
            }
        }
    }
}
