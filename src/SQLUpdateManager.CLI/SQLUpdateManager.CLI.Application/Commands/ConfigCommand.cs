using SQLUpdateManager.CLI.Common;
using SQLUpdateManager.CLI.IO;
using System.Linq;

namespace SQLUpdateManager.CLI.Application
{
    public class ConfigCommand : BaseCommand
    {
        private IParameter _listParameter
        {
            get => _parameters.FirstOrDefault(p => p.Name == Constants.ListParameter);
        }
        private readonly IConfigurationManager _configManager;

        protected override string[] AllowedParameters
        {
            get => new string[]
            {
                Constants.ListParameter
            };
        }

        public override string Name { get => Constants.ConfigCommand; }
        public override bool RequiresParameters { get => false; }
        public override bool RequiresArgument { get => true; }

        public ConfigCommand(IConfigurationManager configManager)
        {
            _configManager = configManager;
        }

        public override void Execute()
        {
            if (_listParameter != null)
            {
                if (!string.IsNullOrEmpty(Argument))
                    throw new InvalidCommandException(ErrorCodes.MissplacedArgument, $"{_listParameter.Name} parameter excludes argument input.");

                var config = _configManager.GetConfig(Constants.ConfigPath);
                foreach (var confValue in _configManager.GetStringConfig(Constants.ConfigPath))
                    Output.PrintLine(confValue);
            }

            else if (string.IsNullOrEmpty(Argument))
                throw new InvalidArgumentException(ErrorCodes.CommandRequiresArgument, $"{Name} command requires argument.");

            else if (!Argument.Contains(".") || !Argument.Contains("="))
                throw new InvalidArgumentException(ErrorCodes.InvalidArgumentFormat,
                    $"{Name} command expects [category].[property]=[value]. Use {Name} command with {Constants.HelpParameter} to get detailed help.");

            else
            {
                var config = _configManager.GetConfig(Constants.ConfigPath);
                var category = GetCategory();
                var property = GetProperty();
                var value = GetValue();

                if (string.IsNullOrEmpty(category))
                    throw new InvalidArgumentException(ErrorCodes.InvalidArgumentFormat, "Category cannot be empty.");

                if (string.IsNullOrEmpty(property))
                    throw new InvalidArgumentException(ErrorCodes.InvalidArgumentFormat, "Property cannot be empty.");

                if (string.IsNullOrEmpty(value))
                    throw new InvalidArgumentException(ErrorCodes.InvalidArgumentFormat, "Value cannot be empty.");

                var updatedCondig = _configManager.UpdateConfig(category, property, value, Constants.ConfigPath);

                Session.Current.UpdateSession(updatedCondig);
            }
        }

        private string GetCategory() =>
            Argument.Substring(0, Argument.IndexOf("."));

        private string GetProperty() =>
            Argument.Substring(Argument.IndexOf(".") + 1, Argument.IndexOf("=") - Argument.IndexOf(".") - 1);

        private string GetValue() =>
            Argument.Substring(Argument.IndexOf("=") + 1);
    }
}
