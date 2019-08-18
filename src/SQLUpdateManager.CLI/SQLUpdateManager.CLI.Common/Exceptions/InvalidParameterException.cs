namespace SqlUpdateManager.CLI.Common
{
    public class InvalidParameterException : CLIException
    {
        protected static string _defaultMessage { get => "Invalid parameter!"; }

        public InvalidParameterException(ErrorCodes code, string message) : base(code, message) { }
        public InvalidParameterException(ErrorCodes code) : base(code, _defaultMessage) { }
    }
}
