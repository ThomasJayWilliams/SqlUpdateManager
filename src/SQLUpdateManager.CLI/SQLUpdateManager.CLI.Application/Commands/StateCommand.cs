using SqlUpdateManager.CLI.Common;
using SqlUpdateManager.CLI.IO;
using System.Linq;

namespace SqlUpdateManager.CLI.Application
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
                _output.PrintLine("Current session values:");

                if (_session.ApplicationStartTimeUtc != null)
                {
                    _output.Print($"Application start time (UTC): ");
					_output.PrintLine(_session.ApplicationStartTimeUtc.ToString());
				}

                if (_session.ApplicationStartTimeLocal != null)
                {
                    _output.Print($"Application start time (local): ");
					_output.PrintLine(_session.ApplicationStartTimeLocal.ToString());
				}

                if (_session.ConnectedServer != null)
                {
                    _output.Print($"Connected server: ");

					var serverName = _session.ConnectedServer.Name.Length > 20 ?
                        $"{_session.ConnectedServer.Name.Substring(0, 19)}..." :
                        _session.ConnectedServer.Name;
                    var serverLocation = _session.ConnectedServer.Location.Length > 20 ?
                        $"{_session.ConnectedServer.Location.Substring(0, 19)}..." :
                        _session.ConnectedServer.Location;
                    var serverUser = _session.ConnectedServer.Username.Length > 20 ?
                        $"{_session.ConnectedServer.Username.Substring(0, 19)}..." :
                        _session.ConnectedServer.Username;

                    _output.PrintLine($"{serverName} {serverLocation} {serverUser}");
				}

                if (_session.UsedDatabase != null)
                {
                    _output.Print($"Database in use: ");
					_output.PrintLine(_session.UsedDatabase.Name.Length > 20 ?
                        $"{_session.UsedDatabase.Name.Substring(0, 19)}..." :
                        _session.UsedDatabase.Name);
				}

                if (_session.Encoding != null)
                {
                    _output.Print($"File encoding: ");
					_output.PrintLine(_session.Encoding.EncodingName);
				}
            }
        }
    }
}
