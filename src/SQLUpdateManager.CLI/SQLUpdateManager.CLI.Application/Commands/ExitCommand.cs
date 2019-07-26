using SQLUpdateManager.CLI.Common;
using System;

namespace SQLUpdateManager.CLI.Application
{
    public class ExitCommand : BaseCommand
    {
        public override string Name { get => Constants.ExitCommand; }
        public override bool RequiresParameters { get => false; }
        public override bool RequiresArgument { get => false; }

        public override void AddParameters(params IParameter[] parameters) =>
            throw new InvalidParameterException(ErrorCodes.UnacceptableParameter, $"{Name} command does not accept parameters.");

        public override void Execute() =>
            Environment.Exit(0);
    }
}
