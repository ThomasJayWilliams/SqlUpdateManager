namespace SqlUpdateManager.CLI.Common
{
    public class InvalidCommandException : CLIException
    {
        protected static string _defaultMessage { get => "Invalid command!"; }

        public InvalidCommandException(ErrorCodes code, string message) : base(code, message) { }
        public InvalidCommandException(ErrorCodes code) : base(code, _defaultMessage) { }
    }
}
