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

        public StateCommand(Session session, IOutput output)
        {
            _output = output;
            _session = session;
        }

        protected override void Validation()
        {
            if (!string.IsNullOrEmpty(Argument))
                throw new InvalidArgumentException(ErrorCodes.MissplacedArgument,
                    $"The {Name} command does not accept arguments.");

            if (_parameters == null || !_parameters.Any())
                throw new InvalidCommandException(ErrorCodes.CommandRequiresParameter,
                    $"{Name} command requires at least one parameter.");
        }

        protected override void Execute()
        {
            if (_listParameter != null)
            {
                _output.PrintColoredLine("Current session values:", _session.Theme.TextColor);

                if (_session.ApplicationStartTimeUtc != null)
                {
                    _output.PrintColored($"Application start time (UTC): ", _session.Theme.PropertyColor);
                    _output.PrintColoredLine(_session.ApplicationStartTimeUtc.ToString(), _session.Theme.TextColor);
                }

                if (_session.ApplicationStartTimeLocal != null)
                {
                    _output.PrintColored($"Application start time (local): ", _session.Theme.PropertyColor);
                    _output.PrintColoredLine(_session.ApplicationStartTimeLocal.ToString(), _session.Theme.TextColor);
                }

                if (_session.ConnectedServer != null)
                {
                    _output.PrintColored($"Connected server: ", _session.Theme.PropertyColor);

                    var serverName = _session.ConnectedServer.Name.Length > 20 ?
                        $"{_session.ConnectedServer.Name.Substring(0, 19)}..." :
                        _session.ConnectedServer.Name;
                    var serverLocation = _session.ConnectedServer.Location.Length > 20 ?
                        $"{_session.ConnectedServer.Location.Substring(0, 19)}..." :
                        _session.ConnectedServer.Location;
                    var serverUser = _session.ConnectedServer.Username.Length > 20 ?
                        $"{_session.ConnectedServer.Username.Substring(0, 19)}..." :
                        _session.ConnectedServer.Username;

                    _output.PrintColoredLine($"{serverName} {serverLocation} {serverUser}",
                        _session.Theme.TextColor);
                }

                if (_session.UsedDatabase != null)
                {
                    _output.PrintColored($"Database in use: ", _session.Theme.PropertyColor);
                    _output.PrintColoredLine(_session.UsedDatabase.Name.Length > 20 ?
                        $"{_session.UsedDatabase.Name.Substring(0, 19)}..." :
                        _session.UsedDatabase.Name, _session.Theme.TextColor);
                }

                if (_session.Encoding != null)
                {
                    _output.PrintColored($"File encoding: ", _session.Theme.PropertyColor);
                    _output.PrintColoredLine(_session.Encoding.EncodingName, _session.Theme.TextColor);
                }

                if (_session.Theme != null)
                {
                    _output.PrintColored($"Current color theme: ", _session.Theme.PropertyColor);
                    _output.PrintColoredLine(_session.Theme.ThemeName, _session.Theme.TextColor);
                }
            }
        }
    }
}
