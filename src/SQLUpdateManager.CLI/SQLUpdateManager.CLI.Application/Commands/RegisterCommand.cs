using SQLUpdateManager.CLI.Common;
using SQLUpdateManager.Core.Registration;
using System.Collections.Generic;

namespace SQLUpdateManager.CLI.Application
{
    public class RegisterCommand : ICommand
    {
        private readonly Register _register;

        public string Name { get => Constants.RegisterCommand; }
        public string Argument { get; set; }
        public bool HasParameters { get => true; }
        public bool RequiresArgument { get => true; }
        public IEnumerable<IParameter> Parameters { get; set; }

        public RegisterCommand(Register register)
        {
            _register = register;
        }

        public void Execute()
        {
            
        }
    }
}
