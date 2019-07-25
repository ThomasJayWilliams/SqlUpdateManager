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

        public abstract void AddParameters(params IParameter[] parameters);

        public void AddParameters(IEnumerable<IParameter> parameters) =>
            AddParameters(parameters.ToArray());

        public abstract void Execute();
    }
}
