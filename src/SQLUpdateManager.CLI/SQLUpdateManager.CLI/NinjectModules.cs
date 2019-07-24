using Ninject.Modules;
using SQLUpdateManager.CLI.Application;
using SQLUpdateManager.Core.Common;
using SQLUpdateManager.Core.Registration;

namespace SQLUpdateManager.CLI
{
    public class MiscModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IMiddleware>().To<ErrorHanlingMiddleware>().InTransientScope();
            Bind<IMiddleware>().To<ApplicationMiddleware>().InTransientScope();

            Bind<ISerializer>().To<JsonSerializer>().InTransientScope();

            Bind<ICommandParser>().To<CommandParser>().InTransientScope();
            Bind<IDataObjectsFactory>().To<DataObjectsFactory>().InTransientScope();
        }
    }

    public class IOModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IPrefixLine>().To<PrefixLine>().InSingletonScope();
        }
    }

    public class CoreModule : NinjectModule
    {
        public override void Load()
        {
            Bind<Register>().ToSelf().InTransientScope();
        }
    }

    public class CommandsModule : NinjectModule
    {
        public override void Load()
        {
            Bind<ICommand>().To<ConnectCommand>().InTransientScope();
            Bind<ICommand>().To<UseCommand>().InTransientScope();
            Bind<ICommand>().To<RegisterCommand>().InTransientScope();
        }
    }
}
