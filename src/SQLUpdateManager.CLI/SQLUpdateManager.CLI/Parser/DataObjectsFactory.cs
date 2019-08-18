using Ninject;
using SqlUpdateManager.CLI.Application;
using SqlUpdateManager.CLI.Common;

namespace SqlUpdateManager.CLI
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
                case CLIConstants.StateCommand:
                    return _serviceProvider.Get<StateCommand>();
                case CLIConstants.ConfigCommand:
                    return _serviceProvider.Get<ConfigCommand>();
                case CLIConstants.RegisterCommand:
                    return _serviceProvider.Get<RegisterCommand>();
                case CLIConstants.ExitCommand:
                    return _serviceProvider.Get<ExitCommand>();
                case CLIConstants.StorageCommand:
                    return _serviceProvider.Get<StorageCommand>();
                default:
                    throw new InvalidCommandException(ErrorCodes.InvalidCommand, $"Command {name} does not exist.");
            }
        }

        public IParameter GetParameter(string name)
        {
            switch (name)
            {
                case CLIConstants.SaveParameter:
                    return _serviceProvider.Get<SaveParameter>();

                case CLIConstants.ListParameter:
                    return _serviceProvider.Get<ListParameter>();
                case CLIConstants.SListParameters:
                    return _serviceProvider.Get<ListParameter>();

                case CLIConstants.DeleteParameter:
                    return _serviceProvider.Get<DeleteParameter>();
                case CLIConstants.SDeleteParameter:
                    return _serviceProvider.Get<DeleteParameter>();

                default:
                    throw new InvalidCommandException(ErrorCodes.InvalidParameter, $"Parameter {name} does not exist.");
            }
        }
    }
}
