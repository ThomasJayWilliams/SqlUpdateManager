using Ninject.Modules;
using SQLUpdateManager.CLI.Application;
using SQLUpdateManager.CLI.Common;
using SQLUpdateManager.Core.Common;
using SQLUpdateManager.Core.Registration;

namespace SQLUpdateManager.CLI
{
    public class CLIModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IMiddleware>().To<ErrorHanlingMiddleware>().InTransientScope();
            Bind<IMiddleware>().To<ApplicationMiddleware>().InTransientScope();

            Bind<ISerializer>().To<JsonSerializer>().InTransientScope();

            Bind<ICommandParser>().To<CommandParser>().InTransientScope();
            Bind<IPrefixLine>().To<PrefixLine>().InSingletonScope();

            Bind<IDataObjectsFactory>().To<DataObjectsFactory>().InTransientScope();
        }
    }

    public class CommonModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IFileManager>().To<FileManager>().InTransientScope();
            Bind<IDataManager>().To<DataManager>().InTransientScope();
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
            Bind<ICommand>().To<ConnectCommand>().InTransientScope();
            Bind<ICommand>().To<UseCommand>().InTransientScope();
            Bind<ICommand>().To<RegisterCommand>().InTransientScope();
        }
    }
}
