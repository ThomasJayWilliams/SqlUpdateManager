using Ninject;
using SQLUpdateManager.CLI.Application;

namespace SQLUpdateManager.CLI
{
    public class CommandParser : ICommandParser
    {
        private readonly IKernel _serviceProvider;

        public CommandParser(IKernel serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public ICommand Parse(string input)
        {
            return null;
        }
    }
}
