using Ninject.Modules;
using SQLUpdateManager.CLI.Application;
using SQLUpdateManager.Core.Common;

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
        }
    }

    public class IOModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IPrefixLine>().To<PrefixLine>().InSingletonScope();
        }
    }

    public class CommandsModule : NinjectModule
    {
        public override void Load()
        {
            Bind<ICommand>().To<ConnectCommand>().InTransientScope();
        }
    }
}
