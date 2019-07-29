using SQLUpdateManager.CLI.Common;
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

        public override void Execute() =>
            Environment.Exit(0);
    }
}
