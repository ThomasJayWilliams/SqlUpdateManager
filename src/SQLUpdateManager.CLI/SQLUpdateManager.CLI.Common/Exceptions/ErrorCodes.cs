namespace SQLUpdateManager.CLI.Common
{
    public enum ErrorCodes
    {
        Unknown = 0,

        InvalidCommand,
        InvalidParameter,
        InvalidArgument,
        InvalidData,
        InvalidEncodingConfiguration,
        InvalidArgumentFormat,

        UnacceptableParameter,
        MissplacedArgument,

        CommandRequiresArgument,

        ProcedureIsAlreadyRegistered,

        ServerIsNotConnected,
        NoDatabaseInUse
    }
}
