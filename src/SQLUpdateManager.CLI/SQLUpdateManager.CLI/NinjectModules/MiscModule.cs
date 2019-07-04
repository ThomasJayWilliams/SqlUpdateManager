using Ninject.Modules;
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
        }
    }
}
