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
        private readonly IOutput _output;

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

        public StateCommand(Session session, IOutput output)
        {
            _output = output;
            _session = session;
        }

        public override void Execute()
        {
            if (_listParameter != null)
            {
                var entries = _session.Entries;

                if (entries != null && entries.Any())
                {
                    _output.PrintColoredLine("Current session values:", _session.Theme.TextColor);

                    foreach (var entry in entries)
                    {
                        _output.PrintColored($"{entry.Name}: ", Color.LightSkyBlue);
                        _output.PrintColoredLine(entry.Value, _session.Theme.TextColor);
                    }
                }

                else
                    _output.PrintColoredLine("Current session is empty.", Color.LightCyan);
            }
        }
    }
}
