using SQLUpdateManager.CLI.Common;
using SQLUpdateManager.CLI.IO;
using System;
using System.Linq;

namespace SQLUpdateManager.CLI.Application
{
    public class StateCommand : BaseCommand
    {
        private IParameter _listParameter
        {
            get => _parameters.FirstOrDefault(p => p.Name == Constants.ListParameter);
        }

        protected override string[] AllowedParameters
        {
            get => new string[]
            {
                Constants.ListParameter
            };
        }

        public override string Name { get => Constants.StateCommand; }
        public override bool RequiresParameters { get => true; }
        public override bool RequiresArgument { get => false; }

        public override void Execute()
        {
            if (_listParameter != null)
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
