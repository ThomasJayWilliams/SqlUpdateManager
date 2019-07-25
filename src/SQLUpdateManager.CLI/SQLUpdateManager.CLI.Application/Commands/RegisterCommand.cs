using SQLUpdateManager.CLI.Common;
using SQLUpdateManager.Core.Registration;
using System;
using System.Linq;

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

        public override void AddParameters(params IParameter[] parameters)
        {
            if (parameters == null || !parameters.Any())
                throw new ArgumentNullException("Parameters cannot be null or empty!");

            _parameters.AddRange(parameters);
        }

        public override void Execute()
        {
            
        }
    }
}
