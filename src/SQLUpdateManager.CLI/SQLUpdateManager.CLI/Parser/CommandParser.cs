using Ninject;
using SQLUpdateManager.CLI.Application;
using SQLUpdateManager.CLI.Common;
using System;
using System.Collections.Generic;

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
            try
            {
                ParseCommand(input);
                ParseParameters(input);
                ParseArguments(input);

                if (_command == null)
                    throw new Exception();

                return _command;
            }
            catch (Exception)
            {
                throw new InvalidCommandException(ErrorCodes.InvalidCommand, "Error appeared while parsing command!");
            }
        }

        private void ParseCommand(string input)
        {
            var nodes = input.Split(' ');

            if (nodes[0] == Constants.SumCommandPrefix)
            {
                switch (nodes[1])
                {
                    case Constants.SumConnect:
                        _command = _serviceProvider.Get<ConnectCommand>();
                        break;
                    default:
                        throw new Exception();
                }
            }

            else
            {

            }
        }

        private void ParseParameters(string input)
        {
            var nodes = input.Split(' ');
            var tempList = new List<IParameter>();

            if (_command is ISUMCommand)
            {
                foreach (var node in nodes)
                {
                    if (node.StartsWith("--"))
                    {
                        switch (node)
                        {
                            case Constants.DoubleHyphenPrefix + Constants.SaveParameter:
                                tempList.Add(_serviceProvider.Get<SaveParameter>());
                                break;
                            default:
                                throw new Exception();
                        }
                    }
                }
            }

            _command.Parameters = tempList;
        }

        private void ParseArguments(string input)
        {
            var nodes = input.Split(' ');
            var argument = _serviceProvider.Get<Argument>();

            argument.Value = nodes[nodes.Length - 1];

            _command.Argument = argument;
        }
    }
}
