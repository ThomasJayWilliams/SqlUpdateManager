using Ninject;
using SQLUpdateManager.CLI.Application;
using SQLUpdateManager.CLI.Common;

namespace SQLUpdateManager.CLI
{
    class DataObjectsFactory : IDataObjectsFactory
    {
        private readonly IKernel _serviceProvider;

        public DataObjectsFactory(IKernel serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public ICommand GetCommand(string name)
        {
            switch (name)
            {
                case Constants.UseCommand:
                    return _serviceProvider.Get<UseCommand>();
                case Constants.ConnectCommand:
                    return _serviceProvider.Get<ConnectCommand>();
                default:
                    throw new InvalidCommandException(ErrorCodes.InvalidCommand, "Cannot parse command.");
            }
        }

        public IParameter GetParameter(string name)
        {
            switch (name)
            {
                case Constants.SaveParameter:
                    return _serviceProvider.Get<SaveParameter>();
                default:
                    throw new InvalidCommandException(ErrorCodes.InvalidParameter, "Cannot parse command parameter.");
            }
        }
    }
}
