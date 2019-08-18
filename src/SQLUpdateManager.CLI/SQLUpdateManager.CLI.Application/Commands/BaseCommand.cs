using SqlUpdateManager.CLI.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SqlUpdateManager.CLI.Application
{
    public abstract class BaseCommand : ICommand
    {
        protected List<IParameter> _parameters = new List<IParameter>();
        protected abstract string[] AllowedParameters { get; }

        public abstract string Name { get; }
        public string Argument { get; set; }
        public IEnumerable<IParameter> Parameters { get => _parameters; }

        public void AddParameters(params IParameter[] parameters)
        {
            if (parameters == null || !parameters.Any())
                throw new ArgumentNullException("Parameters cannot be null or empty!");

            _parameters.AddRange(parameters);
        }

        public void ValidateAndRun()
        {
            BaseValidation();
            Validation();
            Execute();
        }

        private void BaseValidation()
        {
            if (_parameters.Count() != _parameters.Distinct().Count())
                throw new InvalidParameterException(ErrorCodes.DuplicateParameters, "Duplicate parameters.");

            foreach (var param in _parameters)
                if (!AllowedParameters.Any(p => p == param.Name))
                    throw new InvalidParameterException(ErrorCodes.UnacceptableParameter,
                        $"The {Name} command does not accept {param.Name} parameter.");
        }

        public void AddParameters(IEnumerable<IParameter> parameters) =>
            AddParameters(parameters.ToArray());

        protected abstract void Execute();

        protected abstract void Validation();
    }
}
