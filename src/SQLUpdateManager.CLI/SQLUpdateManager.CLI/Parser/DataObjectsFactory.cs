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
                case Constants.StateCommand:
                    return _serviceProvider.Get<StateCommand>();
                case Constants.ConfigCommand:
                    return _serviceProvider.Get<ConfigCommand>();
                case Constants.RegisterCommand:
                    return _serviceProvider.Get<RegisterCommand>();
                case Constants.ExitCommand:
                    return _serviceProvider.Get<ExitCommand>();
                default:
                    throw new InvalidCommandException(ErrorCodes.InvalidCommand, $"Command {name} does not exist.");
            }
        }

        public IParameter GetParameter(string name)
        {
            switch (name)
            {
                case Constants.SaveParameter:
                    return _serviceProvider.Get<SaveParameter>();

                case Constants.ListParameter:
                    return _serviceProvider.Get<ListParameter>();
                case Constants.SListParameters:
                    return _serviceProvider.Get<ListParameter>();

                default:
                    throw new InvalidCommandException(ErrorCodes.InvalidParameter, $"Parameter {name} does not exist.");
            }
        }
    }
}
