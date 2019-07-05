namespace SQLUpdateManager.CLI.Common
{
    public class InvalidStateException : CLIException
    {
        protected static string _defaultMessage { get => "Invalid application state!"; }

        public InvalidStateException(ErrorCodes code, string message) : base(code, message) { }
        public InvalidStateException(ErrorCodes code) : base(code, _defaultMessage) { }
    }
}
