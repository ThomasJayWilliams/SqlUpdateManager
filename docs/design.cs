namespace SUM.CLI
{
    /*
        System -> Application;
        System -> IO;
        System -> Common;

        Application -> Common;
        Application -> IO;


        StartUp -> ErrorHandlingMiddleware -> ApplicationMiddleware -> CommandParser -> ICommand
    */
    namespace System
    {
        class RequestContext
        {
            // Contains input data.
            string Data { get; set; }
        }

        delegate void RequestDelegate(RequestContext context);

        interface IMiddleware
        {
            void OnInvoke(RequestContext context);
        }
        class ErrorHandlingMiddleware : IMiddleware { }
        class ApplicationMiddleware : IMiddleware { }

        class StartUp
        {
            // First middleware in queue.
            IMiddleware InitialMiddleware { get; set; }
            // Initiates appliction configuration - Session, etc.
            void Init();

            static void Main(string[] args);
        }

        static class CommandParser
        {
            IEnumerable<IArgument> ParseArguments(string data);
            IEnumerable<IParameter> ParseParameters(string data);
            ICommand ParseCommand(string data);

            ICommand TryParse(string input);
        }
    }

    namespace IO
    {
        delegate void OutputDelegate(string data);
        delegate string InputDelegate();

        static class InputHandler
        {
            string Read();
        }
        static class OutputHandler
        {            
            void Print(string data);
        }
    }

    namespace Application
    {
        interface IEnvCommand
        {
            IEnumerable<IParameter> Parameters { get; set; }
            IEnumerable<IArgument> Arguments { get; set; }

            void Execute();
        }
        interface ISUMCommand : ICommand { }
        interface IEnvCommand : ICommand { }
        class ExitCommand : IEnvCommand { }
        class StatusCommand : ISUMCommand { }
        ...

        interface IArgument
        {
            string Data { get; set; }
        }
        class Argument : IArgument { }

        interface IParameter
        {
            string Name { get; set; }
            string Value { get; set; }
        }
        class SwitchParameter : IParameter { }
        class ValueParameter : IParameter { }

        class ExitCommandValidator : FluentValidation.AbstractValidator<ExitCommand> { }
        class StatusCommandValidator : FluentValidation.AbstractValidator<StatusCommand> { }
        ...
    }

    namespace Common
    {
        abstract class SUMCLIException : Exception
        {
            ErrorMessage Error { get; set; }

            SUMCLIException(ErrorCodes code);
            SUMCLIException(ErrorCodes code, string message);
        }
        class NotFoundException : SUMCLIException { }
        ...

        enum ErrorCodes
        {
            NotFound = 0,
            ...
        }

        class ErrorMessage
        {
            ErrorCodes Code { get; set; }
            string Message { get; set; }
        }

        static class Session
        {
            static string ServerLocation { get; set; }
            ...
        }
        static class CommandNames
        {
            const string SUM = "sum";

            const string Exit = "exit";
            const string Status = SUM + " status";
            ...
        }
        static class DefaultErrorMessages
        {
            const string SomethingNotFound = "Something not found.";
            ...
        }

        static class Logger
        {
            void LogError(string data);
            void LogEvent(string data);
            void LogExecution(string data);
        }

        static class ServiceContainer
        {
            void Register<TInterface, TImplementation>();
            T GetService<T>();
        }
    }
}

namespace Core
{
    /*
        One of main Core interfaces.
        Manages procedures data and their versions.
        All data stores in single JSON file. Data in file is stored as hash table.
        Each procedure has unique hash, which consists of: "server name"."database name"."procedure name".
        Procedure version represented as Int32 number.
        Examples:
            Versioned: MySQLServer.MyDatabase.MyProcedure.21
            Original: MySQLServer.MyDatabase.MyProcedure
    */
    namespace Tracking
    {
        class Tracker
        {
            TrackingFileManager FileManager { get; set; }
            RegistrationLoader Loader { set; set; }

            Tracker(TrackingFileManager fileManager, RegistrationLoader loader);

            bool IsChanged();
            IEnumerable<Procedure> GetChanged();
            Procedure LoadVersion(string hash, int version);
            Procedure LoadHead(string hash);
            void SaveVersion(Procedure procedure);
            Difference GetDifference(string sourceHash, string destinationHash,
                int sourceVersion, int destinationVersion,
                DifferenceFormat format);
            Difference GetDifference(string hash, int sourceVersion,
                Procedure destination);
        }
        class Difference
        {
            Procedure Older { get; set; }
            Procedure Newer { get; set; }
            string Comparasion { get; set; }
        }
        static class Compressor
        {
            string Compress(string data);
            string Decompress(string data);
        }
    }

    /*
        Used to registrate procedures for tracking.
        All registrations contains in single JSON file.
    */
    namespace Registration
    {
        class Registrator
        {
            RegistrationFileManager FileManager { get; set; }
            Server[] Servers { get; set; }

            Registrator(RegistrationFileManager fileManager);

            void RegistrateServers(Server[] data);
            void RegistrateDatabases(Database[] data);
            void RegistrateProcedures(Procedure[] data);

            void Save(string path);
        }
        class RegistrationLoader
        {
            RegistrationFileManager FileManager { get; set; }

            RegistrationLoader(RegistrationFileManager fileManager);

            Server LoadServer(string hash);
            Database LoadDatabase(string hash);
            Procedure LoadProcedure(string hash);
            Server[] LoadServers();
            Database[] LoadDatabases();
            Database[] LoadDatabases(string serverHash);
            Procedure[] LoadProcedures();
            Procedure[] LoadProcedures(string databaseHash);
            IData Search(string hash);
        }
    }

    namespace Common
    {
        class JsonSerializer
        {
            T Deserialize<T>(string json);
            string Serialize(object data);
        }
        class Crypter
        {
            string Encrypt(string data);
            string Decryot(string hash);
        }
        class RegistrationFileManager
        {
            JsonSerializer Serializer { get; set; }
            string Path { get; set; }

            RegistrationFileManager(string path, JsonSerializer serializer);

            void Save(Server[] servers);
            Server[] Load();
        }
        class TrackingFileManager
        {
            JsonSerializer Serializer { get; set; }
            string Path { get; set; }

            TrackingFileManager(string path, JsonSerializer serializer);

            ProceduresList LoadProcedure(string hash);
            IEnumerable<Procedure> LoadProcedures();
            void Save(IEnumerable<Procedure> procedures);
        }
    }

    namespace Data
    {
		class ProcedureExecutor
		{
			ExecutionResult Execute(Procedure procedure);
		}
    }

    namespace Domains
    {
        // Linked list, contains all versions of single procedure.
        class ProceduresList
        {
            Procedure Original { get; set; }
            Node<Procedure> Next { get; set; }
            Node<Procedure> Previous { get; set; }

            void NewVersion(Procedure procedure);
        }
        interface IData
        {
            string Name { get; set; }
            string Hash { get; set; }
        }
        class Server : IData
        {
            Database[] Databases { get; set; }
            string Location { get; set; }
            ServerUser[] Users { get; set; }
        }
        class Database : IData
        {
            Procedure[] Procedures { get; set; }
        }
        class Procedure : IData
        {
            string Data { get; set; }
            string Location { get; set; }
        }
        class ServerUser
        {
            string Username { get; set; }
            string Password { get; set; }
        }
		class ExecutionResult
		{
			bool IsSuccess { get; set; }
			Exception Error { get; set; }
		}
        enum DifferenceFormat
        {
            Compared = 0,
            FullCompared,
            ...
        }
    }
}