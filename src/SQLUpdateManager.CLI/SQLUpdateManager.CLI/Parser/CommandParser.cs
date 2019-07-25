using SQLUpdateManager.CLI.Application;
using SQLUpdateManager.CLI.Common;
using System.Collections.Generic;
using System.Linq;

namespace SQLUpdateManager.CLI
{
    public class CommandParser : ICommandParser
    {
        private readonly IDataObjectsFactory _objectsFactory;
        private ICommand _command;

        public CommandParser(IDataObjectsFactory objectsFactory)
        {
            _objectsFactory = objectsFactory;
        }

        public ICommand Parse(string input)
        {
            var nodes = input.Split(' ').ToList();

            if (nodes.Count < 1)
                throw new InvalidCommandException(ErrorCodes.InvalidCommand, "Could not parse the command.");

            ParseCommand(nodes);
            ParseParameters(nodes);

            if (_command == null)
                throw new InvalidCommandException(ErrorCodes.InvalidCommand,
                    $"Error parsing command. Check {Constants.ErrorLogPath} for error log.");

            if (_command.RequiresArgument)
                _command.Argument = nodes.LastOrDefault();

            return _command;
        }

        private void ParseCommand(List<string> input)
        {
            var command = input.FirstOrDefault();
            _command = _objectsFactory.GetCommand(command);
            input.RemoveAt(0);
        }
        
        private void ParseParameters(List<string> input)
        {
            if (input != null && input.Count > 0)
            {
                var list = new List<IParameter>();

                foreach (var node in input)
                {
                    if (node.StartsWith(Constants.DParameterPrefix))
                    {
                        IParameter param;

                        if (node.Contains("="))
                        {
                            param = _objectsFactory.GetParameter(node.Substring(Constants.DParameterPrefix.Length, node.IndexOf("=") - 2));

                            if (param.RequiresArgument)
                                param.Argument = node.Substring(node.IndexOf("="));

                            else
                                throw new InvalidCommandException(ErrorCodes.InvalidParameter, $"The {param.Name} parameter does not accept arguments.");
                        }

                        else
                        {
                            param = _objectsFactory.GetParameter(node.Substring(Constants.DParameterPrefix.Length));

                            if (param.RequiresArgument)
                                throw new InvalidCommandException(ErrorCodes.InvalidParameter, $"The {param.Name} parameter requires argument.");
                        }

                        list.Add(param);
                    }

                    else if (node.StartsWith("-"))
                        list.AddRange(node.Skip(1).Select(c => _objectsFactory.GetParameter(c.ToString())));
                }

                if (list.Any())
                    _command.AddParameters(list);
            }
        }
    }
}
