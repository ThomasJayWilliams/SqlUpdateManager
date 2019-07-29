using SQLUpdateManager.CLI.Common;
using SQLUpdateManager.CLI.IO;
using System;

namespace SQLUpdateManager.CLI.Application
{
    public class ExitCommand : BaseCommand
    {
        private readonly ILogger _logger;

        protected override string[] AllowedParameters
        {
            get => new string[] { };
        }

        public override string Name { get => CLIConstants.ExitCommand; }
        public override bool RequiresParameters { get => false; }
        public override bool RequiresArgument { get => false; }

        public ExitCommand(ILogger logger)
        {
            _logger = logger;
        }

        public override void Execute()
        {
            _logger.LogInfo("Exiting with code 0.");
            Environment.Exit(0);
        }
    }
}
