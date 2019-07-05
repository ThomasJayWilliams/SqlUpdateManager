using Ninject;
using SQLUpdateManager.CLI.Application;
using SQLUpdateManager.CLI.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SQLUpdateManager.CLI
{
    public class CommandParser : ICommandParser
    {
        private readonly IKernel _serviceProvider;
        private ICommand _command;

        public CommandParser(IKernel serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public ICommand Parse(string input)
        {
            ParseCommand(input);
            ParseParameters(input);
            ParseArguments(input);

            if (_command == null)
                throw new Exception();

            return _command;
        }

        private void ParseCommand(string input)
        {
            var nodes = input.Split(' ');

            switch (nodes[0])
            {
                case Constants.Connect:
                    _command = _serviceProvider.Get<ConnectCommand>();
                    break;
                case Constants.Use:
                    _command = _serviceProvider.Get<UseCommand>();
                    break;
                default:
                    throw new InvalidCommandException(ErrorCodes.InvalidCommand, "The command cannot be parsed!");
            }
        }

        private void ParseParameters(string input)
        {
            var nodes = input.Split(' ');
            var tempList = new List<IParameter>();

            foreach (var node in nodes)
            {
                if (node.StartsWith("--"))
                {
                    switch (node)
                    {
                        case Constants.DSaveParameter:
                            tempList.Add(_serviceProvider.Get<SaveParameter>());
                            break;
                        default:
                            throw new InvalidCommandException(ErrorCodes.InvalidParameter, "The command parameters cannot be parsed!");
                    }
                }

                else if (node.StartsWith('-'))
                {
                    foreach (var letter in node)
                    {
                        switch (letter)
                        {
                            case Constants.SSaveParameter:
                                tempList.Add(_serviceProvider.Get<SaveParameter>());
                                break;
                            default:
                                throw new InvalidCommandException(ErrorCodes.InvalidParameter, "The command parameters cannot be parsed!");
                        }
                    }
                }
            }

            if (tempList.Distinct().Count() < tempList.Count())
                throw new InvalidCommandException(ErrorCodes.InvalidParameter, "The same parameter cannot be passed mroe than one time!");

            _command.Parameters = tempList;
        }

        private void ParseArguments(string input)
        {
            var nodes = input.Split(' ');
            var arg = _serviceProvider.Get<Argument>();

            if (nodes.Length > 1)
                arg.Value = nodes[nodes.Length - 1];

            _command.Argument = arg;
        }
    }
}
