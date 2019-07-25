using SQLUpdateManager.CLI.Common;
using SQLUpdateManager.CLI.IO;
using System;
using System.Linq;

namespace SQLUpdateManager.CLI.Application
{
    public class StateCommand : BaseCommand
    {
        private ShowParameter _showParameter;

        public override string Name { get => Constants.StateCommand; }

        public override bool RequiresParameters { get => true; }
        public override bool RequiresArgument { get => false; }

        public override void AddParameters(params IParameter[] parameters)
        {
            if (parameters == null || !parameters.Any())
                throw new ArgumentNullException("Parameters cannot be null or empty!");

            foreach (var param in parameters)
            {
                if (param.Name == Constants.ShowParameter)
                    _showParameter = param as ShowParameter;
                else
                    throw new InvalidParameterException(ErrorCodes.UnacceptableParameter, $"{Name} command does not accept {param.Name} parameter.");
            }

            _parameters.AddRange(parameters);
        }

        public override void Execute()
        {
            if (_showParameter != null)
            {
                var entries = Session.Current.Entries;

                if (entries != null && entries.Any())
                {
                    Output.PrintLine("Current session values:");

                    foreach (var entry in entries)
                    {
                        Output.PrintColored($"{entry.Name}: ", ConsoleColor.Cyan);
                        Output.PrintLine(entry.Value);
                    }
                }

                else
                    Output.PrintColoredLine("Current session is empty.", ConsoleColor.Red);
            }
        }
    }
}
