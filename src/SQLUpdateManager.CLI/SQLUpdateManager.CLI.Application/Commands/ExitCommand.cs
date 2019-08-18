using SqlUpdateManager.CLI.Common;
using SqlUpdateManager.CLI.IO;
using System;

namespace SqlUpdateManager.CLI.Application
{
    public class ExitCommand : BaseCommand
    {
        private readonly ILogger _logger;

        protected override string[] AllowedParameters
        {
            get => new string[] { };
        }

        public override string Name { get => CLIConstants.ExitCommand; }

        public ExitCommand(ILogger logger)
        {
            _logger = logger;
        }

        protected override void Validation() { }

        protected override void Execute()
        {
            _logger.LogInfo("Exiting with code 0.");
            Environment.Exit(0);
        }
    }
}
