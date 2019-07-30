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
        DuplicateParameters,
        MissplacedArgument,

        CommandRequiresArgument,
        CommandRequiresParameter,

        ProcedureIsAlreadyRegistered,

        ServerIsNotConnected,
        AlreadyConnectedToTheServer,
        NoDatabaseInUse,

        ServerIsNotRegistered,
        DatabaseIsNotRegistered
    }
}
