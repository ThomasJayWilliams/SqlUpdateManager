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
        ConflictParameters,
        MissplacedArgument,

        ProcedureFileIsNotFound,

        CommandRequiresArgument,
        CommandRequiresParameter,

        ProcedureIsAlreadyRegistered,
        ServerIsAlreadyRegistered,
        DatabaseIsAlreadyRegistered,

        ServerIsNotConnected,
        AlreadyConnectedToTheServer,
        NoDatabaseInUse,
        AlreadyUsingDatabase,

        ServerIsNotRegistered,
        DatabaseIsNotRegistered,
        ProcedureIsNotRegistered
    }
}
