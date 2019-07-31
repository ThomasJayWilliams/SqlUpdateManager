using SQLUpdateManager.CLI.Common;
using SQLUpdateManager.CLI.IO;
using System.Linq;

namespace SQLUpdateManager.CLI.Application
{
    public class ConfigCommand : BaseCommand
    {
        private IParameter _listParameter
        {
            get => _parameters.FirstOrDefault(p => p.Name == CLIConstants.ListParameter);
        }

        private readonly IConfigurationManager _configManager;
        private readonly IOutput _output;
        private readonly Session _session;

        protected override string[] AllowedParameters
        {
            get => new string[]
            {
                CLIConstants.ListParameter
            };
        }

        public override string Name { get => CLIConstants.ConfigCommand; }

        public ConfigCommand(IConfigurationManager configManager, Session session, IOutput output)
        {
            _output = output;
            _session = session;
            _configManager = configManager;
        }

        protected override void Validation()
        {
            if (_listParameter != null)
            {
                if (!string.IsNullOrEmpty(Argument))
                    throw new InvalidCommandException(ErrorCodes.MissplacedArgument, $"{_listParameter.Name} parameter excludes argument input.");
            }

            else if (string.IsNullOrEmpty(Argument))
                throw new InvalidArgumentException(ErrorCodes.CommandRequiresArgument,
                    $"{Name} command requires argument.");

            else if (!Argument.Contains(".") || !Argument.Contains("="))
                throw new InvalidArgumentException(ErrorCodes.InvalidArgumentFormat,
                    $"{Name} command expects [category].[property]=[value].");
        }

        protected override void Execute()
        {
            if (_listParameter != null)
            {
                var config = _configManager.GetConfig(CLIConstants.ConfigPath);
                foreach (var confValue in _configManager.GetStringConfig(CLIConstants.ConfigPath))
                    _output.PrintColoredLine(confValue, _session.Theme.TextColor);
            }
            
            else
            {
                var config = _configManager.GetConfig(CLIConstants.ConfigPath);
                var category = GetCategory();
                var property = GetProperty();
                var value = GetValue();

                if (string.IsNullOrEmpty(category))
                    throw new InvalidArgumentException(ErrorCodes.InvalidArgumentFormat, "Category cannot be empty.");

                if (string.IsNullOrEmpty(property))
                    throw new InvalidArgumentException(ErrorCodes.InvalidArgumentFormat, "Property cannot be empty.");

                if (string.IsNullOrEmpty(value))
                    throw new InvalidArgumentException(ErrorCodes.InvalidArgumentFormat, "Value cannot be empty.");

                var updatedCondig = _configManager.UpdateConfig(category, property, value, CLIConstants.ConfigPath);

                UpdateSession(updatedCondig);
            }
        }

        private void UpdateSession(AppConfig config)
        {
            if (!string.IsNullOrEmpty(config.Core.FileEncoding))
                _session.Encoding = EncodingHelper.GetEncoding(config.Core.FileEncoding);
        }

        private string GetCategory() =>
            Argument.Substring(0, Argument.IndexOf("."));

        private string GetProperty() =>
            Argument.Substring(Argument.IndexOf(".") + 1, Argument.IndexOf("=") - Argument.IndexOf(".") - 1);

        private string GetValue() =>
            Argument.Substring(Argument.IndexOf("=") + 1);
    }
}
