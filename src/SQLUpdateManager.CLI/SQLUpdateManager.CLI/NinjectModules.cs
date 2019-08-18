using Ninject.Modules;
using SqlUpdateManager.CLI.Application;
using SqlUpdateManager.CLI.Common;
using SqlUpdateManager.CLI.IO;
using SqlUpdateManager.Core.Common;
using SqlUpdateManager.Core.Registry;

namespace SqlUpdateManager.CLI
{
    public class CLIModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IMiddleware>().To<ErrorHanlingMiddleware>().InTransientScope();
            Bind<IMiddleware>().To<ApplicationMiddleware>().InTransientScope();

            Bind<ICommandParser>().To<CommandParser>().InTransientScope();

            Bind<IDataObjectsFactory>().To<DataObjectsFactory>().InTransientScope();

            Bind<IIOConfiguration>().To<IOConfiguration>().InTransientScope();
            Bind<ICommonConfiguration>().To<CommonConfiguration>().InTransientScope();

            Bind<ILogger>().To<Logger>().InTransientScope();

            Bind<IOutput>().To<Output>().InSingletonScope();
            Bind<IInput>().To<Input>().InSingletonScope();

            Bind<Session>().ToSelf().InSingletonScope();
            Bind<Startup>().ToSelf().InTransientScope();
        }
    }

    public class CommonModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IDataRepository>().To<DataRepository>().InTransientScope();
            Bind<IConfigurationManager>().To<ConfigurationManager>().InTransientScope();

            Bind<ISerializer>().To<JsonSerializer>().InTransientScope();
            Bind<IFileManager>().To<FileManager>().InTransientScope();
        }
    }

    public class CoreModule : NinjectModule
    {
        public override void Load()
        {
            Bind<Register>().ToSelf().InTransientScope();
        }
    }

    public class ApplicationModule : NinjectModule
    {
        public override void Load()
        {
            Bind<ICommand>().To<RegisterCommand>().InTransientScope();
            Bind<ICommand>().To<ConfigCommand>().InTransientScope();
            Bind<ICommand>().To<ExitCommand>().InTransientScope();
            Bind<ICommand>().To<StateCommand>().InTransientScope();
            Bind<ICommand>().To<StorageCommand>().InTransientScope();

            Bind<IParameter>().To<ListParameter>().InTransientScope();
            Bind<IParameter>().To<SaveParameter>().InTransientScope();
            Bind<IParameter>().To<DeleteParameter>().InTransientScope();
        }
    }
}
