namespace SUM.CLI
{
    namespace System
    {
		// Dependencies.
		using Common;
		using IO;
		using Application;

        // Contains request context, which is used by middleware.
        // Can contain data about request time and etc.
        class RequestContext
        {
            // Contains input data.
            public string Data { get; set; }

			RequestContext(params RequestDelegate[] delegates) { }

			public event RequestDelegate Next { get; private set; }
        }

        // Used as a reference on next item in request chain.
        delegate void RequestDelegate(RequestContext context);

        // Represent sinteface for call chain items.
        interface IMiddleware
        {
            void OnInvoke(RequestContext context);
        }
        class InitialMiddleware : IMiddleware
		{
			public void OnInvoke(RequestDelegate context);
		}
        class ErrorHandlingMiddleware : IMiddleware
		{
			public void OnInvoke(RequestDelegate context);
		}
        class ApplicationMiddleware : IMiddleware
		{
			CommandParser Parser { get; set; }

			public void OnInvoke(RequestContext context);
		}

        class StartUp
        {
            // First middleware in queue.
            IMiddleware InitialMiddleware { get; set; }
            // Initiates appliction configuration - Session, etc.
            void Init(string[] args);
			void ConfigureServices();
			void Configure(AppConfig configuration);
            void RunApp();
        }

		class Program
		{
			// Entry point.
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
		// Dependencies.
		using IO;
		using Common;
		using Core.Domains;
		using Core.Registration;
		using Core.Tracking;
		using Core.Data;
		using Core.Common;

        class CommandParser
        {
			private IEnumerable<IArgument> ParseArguments(string data);
			private IEnumerable<IParameter> ParseParameters(string data);

			ICommand ParseCommand(string data);
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

        class ServiceContainer
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
		// Dependencies.
		using Core.Domains;
		using Core.Common;
		using Core.Internal;

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
		// Registartion.
		using Core.Domains;
		using Core.Common;
		using Core.Internal;

        class Register
        {
            string Path { get; set; }
            IEnumerable<Server> Servers { get; set; }
            ISerializer Serializer { get; set; }

            Register(ISerializer serializer, string path);

            void AddServer(Server data);
            void RemoveServer(string hash);
            void UdpateServer(Server data);
            Server GetServer(string hash);

            void SaveChanges();
        }
    }

    namespace Internal
    {
        internal class Hasher
        {
            string GetHash(string data);
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
    }

    namespace Common
    {
        interface ISerializer
        {
            T Deserialize<T>(string data);
            string Serialize(object data);
        }
    }

    namespace Data
    {
		// Dependencies.
		using Core.Domains;

		class ProcedureExecutor
		{
            // Wonder if this is possible.
            bool EstablishConnection(Server server);
			ExecutionResultMessage Execute(Procedure procedure);
		}
    }

    namespace Domains
    {
        interface IMessage
        {

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
		class ExecutionResultMessage : IMessage
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