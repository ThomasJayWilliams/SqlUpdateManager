using System;

namespace SQLUpdateManager.CLI.Common
{
    public class CLIException : Exception
    {
        public ErrorCodes Code { get; set; }

        public CLIException(ErrorCodes code, string message) : base(message)
        {
            Code = code;
        }
    }
}
