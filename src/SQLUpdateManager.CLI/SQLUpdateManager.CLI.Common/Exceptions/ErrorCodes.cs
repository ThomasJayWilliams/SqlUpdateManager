namespace SQLUpdateManager.CLI.Common
{
    public enum ErrorCodes
    {
        Unknown = 0,

        InvalidCommand,
        InvalidParameter,
        InvalidArgument,
        InvalidData,

        UnacceptableParameter,

        InvalidEncodingConfiguration,
        InvalidArgumentFormat,

        CommandRequiresArgument,

        ServerIsNotConnected
    }
}
