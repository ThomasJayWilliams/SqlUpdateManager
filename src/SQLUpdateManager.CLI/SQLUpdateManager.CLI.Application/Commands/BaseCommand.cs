using SQLUpdateManager.CLI.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SQLUpdateManager.CLI.Application
{
    public abstract class BaseCommand : ICommand
    {
        protected List<IParameter> _parameters = new List<IParameter>();

        public abstract string Name { get; }
        public abstract bool RequiresParameters { get; }
        public abstract bool RequiresArgument { get; }

        protected abstract string[] AllowedParameters { get; }

        public string Argument { get; set; }
        public IEnumerable<IParameter> Parameters { get => _parameters; }

        public void AddParameters(params IParameter[] parameters)
        {
            if (parameters == null || !parameters.Any())
                throw new ArgumentNullException("Parameters cannot be null or empty!");

            _parameters.AddRange(parameters);

            ValidateParameters();
        }

        protected void ValidateParameters()
        {
            if (_parameters.Count() != _parameters.Distinct().Count())
                throw new InvalidParameterException(ErrorCodes.DuplicateParameters, "Duplicate parameters.");

            foreach (var param in _parameters)
                if (!AllowedParameters.Any(p => p == param.Name))
                    throw new InvalidParameterException(ErrorCodes.UnacceptableParameter,
                        $"{Name} command does not accept {param.Name} parameter.");
        }

        public void AddParameters(IEnumerable<IParameter> parameters) =>
            AddParameters(parameters.ToArray());

        public abstract void Execute();
    }
}
