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

        public string Argument { get; set; }
        public IEnumerable<IParameter> Parameters { get => _parameters; }

        public void AddParameters(params IParameter[] parameters)
        {
            if (parameters == null || !parameters.Any())
                throw new ArgumentNullException("Parameter cannot be null!");

            _parameters.AddRange(parameters);
        }

        public void AddParameters(IEnumerable<IParameter> parameters) =>
            AddParameters(parameters.ToArray());

        public abstract void Execute();
    }
}
