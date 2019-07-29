using SQLUpdateManager.CLI.Common;
using SQLUpdateManager.CLI.IO;
using System;

namespace SQLUpdateManager.CLI.Application
{
    public class ExitCommand : BaseCommand
    {
        protected override string[] AllowedParameters
        {
            get => new string[] { };
        }

        public override string Name { get => Constants.ExitCommand; }
        public override bool RequiresParameters { get => false; }
        public override bool RequiresArgument { get => false; }

        public override void Execute()
        {
            SerilogLogger.LogInfo("Exiting with code 0.");
            Environment.Exit(0);
        }
    }
}
