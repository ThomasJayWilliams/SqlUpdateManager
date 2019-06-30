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
        // Contains request context, which is used by middleware.
        // Can contain data about request time and etc.
        class RequestContext
        {
            // Contains input data.
            string Data { get; set; }

			event RequestDelegate Next { get; set; }
        }

        // Used as a reference on next item in request chain.
        delegate void RequestDelegate(RequestContext context);

        // Represent sinteface for call chain items.
        interface IMiddleware
        {
            void OnInvoke(RequestContext context);
        }
        class InitialMiddleware : IMiddleware { }
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
    }

    namespace IO
    {
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
        static class CommandParser
        {
            IEnumerable<IArgument> ParseArguments(string data);
            IEnumerable<IParameter> ParseParameters(string data);
            ICommand ParseCommand(string data);

            ICommand TryParse(string input);
        }

        interface ICommand
        {
            string Name { get; set; }
            IEnumerable<IParameter> Parameters { get; set; }
            IEnumerable<IArgument> Arguments { get; set; }

            void Execute();
        }
        interface IEnvCommand : ICommand { }
        interface ISumCommand : ICommand { }
        class ExitCommand : IEnvCommand { }
        class StatusCommand : ISumCommand { }
		// ...

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
		// ...
	}

	namespace Common
    {
        abstract class SumCLIException : Exception
        {
            ErrorMessage Error { get; set; }

            SumCLIException(ErrorCodes code);
            SumCLIException(ErrorCodes code, string message);
        }
        class NotFoundException : SumCLIException { }
		// ...

		enum ErrorCodes
        {
            NotFound = 0,
			// ...
		}

		class ErrorMessage
        {
            ErrorCodes Code { get; set; }
            string Message { get; set; }
        }

        // Singleton.
        class Session
        {
            static string ConnectedServerLocation { get; set; }
            static string ConnectedServerUsername { get; set; }
			// ...
		}
		static class CommandNames
        {
            const string Sum = "sum";

            const string Exit = "exit";
            const string Status = Sum + " status";
			// ...
		}
		static class DefaultErrorMessages
        {
            const string SomethingNotFound = "Something is not found.";
			// ...
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

        static class Locations
        {
            const string AppConfigLocation = "...";
            const string AppCacheLocation = "...";
        }

        class ConfigurationManager<T>
        {
            string Path { get; set; }

            ConfigurationManager(string path);

            T LoadConfig();
            void SaveConfig(T config);
        }
        class AppConfig
        {
            string SessionLifeTime { get; set; }
			// ...
		}
		class AppCache
        {
            string ServerLocation { get; set; }
            string ServerName { get; set; }
            string ServerUsername { get; set; }
            string ServerPassword { get; set; }
			// ...
		}
	}
}

namespace Core
{
    /*
        One of main Core interfaces.
        Manages procedures data and their versions.
        All data stores in single file. Data in file is stored as hash table.
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
            Procedure GetRevision(string revHash);
            Procedure GetHead(string sourceHash);
            void Save(Procedure procedure);
        }
        class Comparer
        {
            Difference GetDifference(string sourceHash, string destinationHash,
                DifferenceFormat format);
        }
        class Monitor
        {
            bool IsChanged();
            IEnumerable<Procedure> GetChanged();
        }
    }

    /*
        Used to register procedures for tracking.
        All registrations contains in single file.
    */
    namespace Registration
    {
        class Register
        {
            string Path { get; set; }
            Server[] Servers { get; set; }

            Registrator(string path);

            void RegisterServer(Server data);
            void RegisterDatabase(Database data);
            void RegisterProcedure(Procedure data);

            void DeregisterServer(Server data);
            void DeregisterDatabase(Database data);
            void DeregisterProcedure(Procedure data);

            Server GetServer(string hash);
            Database GetDatabase(string hash);
            Procedure GetProcedure(string hash);
            IData Search(string hash);

            void SaveChanges();
        }
    }

    namespace Internal
    {
        internal class Crypter
        {
            string Encrypt(string data);
            string Decrypt(string hash);
        }
        // Singleton.
        internal class FileManager
        {
            string Load(string path);
            void Save(string path);
        }
        internal class Compressor
        {
            string Compress(string data);
            string Decompress(string data);
        }
        // Singleton.
        internal class ConfigurationManager
        {
            ISerializer Tracking { get; set; }
            ISerializer Register { get; set; }
        }
    }

    namespace Common
    {
        interface ISerializer
        {
            T Deserialize<T>(string data);
            string Serialize(object data);
        }
        class CoreConfiguration
        {
            void SetTrackingSerializer(ISerializer serializer);
            void SetRegisterSerializer(ISerializer serializer);
        }
    }

    namespace Data
    {
		class ProcedureExecutor
		{
            // Wonder if this is possible
            bool EstablishConnection(Server server);
			ExecutionResult Execute(Procedure procedure);
		}
    }

    namespace Domains
    {
        interface IData
        {
            string Name { get; set; }
            string Hash { get; set; }
        }
        class Server : IData
        {
            Database[] Databases { get; set; }
            string Location { get; set; }
            ServerType Type { get; set; }
            ServerUser[] Users { get; set; }
        }
        class Database : IData
        {
            Server Server { get; set; }
            Procedure[] Procedures { get; set; }
        }
        class Procedure : IData
        {
            Database Database { get; set; }
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
        class Difference
        {
            string Comparasion { get; set; }
            DifferenceFormat Format { get; set; }
        }
        enum DifferenceFormat
        {
            Compared = 0,
            FullCompared,
            // ...
        }
        enum ServerType
        {
            SqlServer = 0,
            MySql,
			// ...
		}
	}
}