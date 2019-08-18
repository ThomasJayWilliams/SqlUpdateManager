namespace SqlUpdateManager.CLI.Common
{
    public class InvalidConfigurationException : CLIException
    {
        protected static string _defaultMessage { get => "Invalid configuration settings!"; }

        public InvalidConfigurationException(ErrorCodes code, string message) : base(code, message) { }
        public InvalidConfigurationException(ErrorCodes code) : base(code, _defaultMessage) { }
    }
}
