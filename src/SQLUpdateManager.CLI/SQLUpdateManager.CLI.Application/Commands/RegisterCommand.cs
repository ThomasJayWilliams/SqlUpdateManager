using SQLUpdateManager.CLI.Common;
using SQLUpdateManager.Core.Registration;

namespace SQLUpdateManager.CLI.Application
{
    public class RegisterCommand : BaseCommand
    {
        private readonly Register _register;

        public override string Name { get => Constants.RegisterCommand; }
        public override bool RequiresParameters { get => false; }
        public override bool RequiresArgument { get => true; }

        public RegisterCommand(Register register)
        {
            _register = register;
        }

        public override void Execute()
        {
            
        }
    }
}
