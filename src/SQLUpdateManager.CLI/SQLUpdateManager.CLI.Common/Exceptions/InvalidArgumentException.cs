namespace SQLUpdateManager.CLI.Common
{
    public class InvalidArgumentException : CLIException
    {
        protected static string _defaultMessage { get => "Invalid command argument."; }

        public InvalidArgumentException(ErrorCodes code, string message) : base(code, message) { }
        public InvalidArgumentException(ErrorCodes code) : base(code, _defaultMessage) { }
    }
}
