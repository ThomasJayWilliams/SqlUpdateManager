using SQLUpdateManager.CLI.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace SQLUpdateManager.CLI.IO
{
    public class Prefix : IPrefix
    {
        private readonly Session _session;

        public Prefix(Session session)
        {
            _session = session;
        }

        public void PrintPrefix()
        {
            var theme = _session.Theme;

            if (_session.ConnectedServer != null)
            {
                Output.PrintColored(CLIConstants.AppName, theme.AppColor);

                Output.Print(" ");
                Output.PrintColored(
                    _session.ConnectedServer.Name.Length > 25 ?
                        $"{_session.ConnectedServer.Name.Substring(0, 25)}..." :
                        _session.ConnectedServer.Name,
                    theme.ServerColor);

                if (_session.UsedDatabase != null)
                {
                    Output.PrintColored("/", theme.DatabaseColor);
                    Output.PrintColoredLine(
                        _session.UsedDatabase.Name.Length > 25 ?
                            $"({_session.UsedDatabase.Name.Substring(0, 25)}...)" :
                            _session.UsedDatabase.Name,
                        theme.DatabaseColor);
                }

                else
                    Output.PrintEmptyLine();
            }

            else
                Output.PrintColoredLine(CLIConstants.AppName, theme.AppColor);

            Output.Print($"{CLIConstants.PrefixSymbol} ");
        }
    }
}
