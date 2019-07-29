using SQLUpdateManager.CLI.Common;
using SQLUpdateManager.CLI.IO;
using System.Drawing;
using System.Linq;

namespace SQLUpdateManager.CLI.Application
{
    public class StateCommand : BaseCommand
    {
        private IParameter _listParameter
        {
            get => _parameters.FirstOrDefault(p => p.Name == CLIConstants.ListParameter);
        }

        private readonly Session _session;

        protected override string[] AllowedParameters
        {
            get => new string[]
            {
                CLIConstants.ListParameter
            };
        }

        public override string Name { get => CLIConstants.StateCommand; }
        public override bool RequiresParameters { get => true; }
        public override bool RequiresArgument { get => false; }

        public StateCommand(Session session)
        {
            _session = session;
        }

        public override void Execute()
        {
            if (_listParameter != null)
            {
                var entries = _session.Entries;

                if (entries != null && entries.Any())
                {
                    Output.PrintLine("Current session values:");

                    foreach (var entry in entries)
                    {
                        Output.PrintColored($"{entry.Name}: ", Color.LightSkyBlue);
                        Output.PrintLine(entry.Value);
                    }
                }

                else
                    Output.PrintColoredLine("Current session is empty.", Color.LightCyan);
            }
        }
    }
}
