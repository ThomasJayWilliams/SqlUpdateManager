using SQLUpdateManager.CLI.Common;
using SQLUpdateManager.CLI.IO;
using System;

namespace SQLUpdateManager.CLI
{
    public class PrefixLine : IPrefixLine
    {
        public void PrintPrefix()
        {
            if (Session.Current.ConnectedServer != null)
            {
                Output.PrintColored(Constants.AppName, ConsoleColor.Green);

                Output.Print(" ");
                Output.PrintColored(
                    Session.Current.ConnectedServer.Name.Length > 25 ?
                        $"{Session.Current.ConnectedServer.Name.Substring(0, 25)}..." :
                        Session.Current.ConnectedServer.Name,
                    ConsoleColor.Yellow);

                if (Session.Current.UsedDatabase != null)
                {
                    Output.Print("/");
                    Output.PrintColoredLine(
                        Session.Current.UsedDatabase.Name.Length > 25 ?
                            $"({Session.Current.UsedDatabase.Name.Substring(0, 25)}...)" :
                            Session.Current.UsedDatabase.Name,
                        ConsoleColor.Cyan);
                }

                else
                    Output.PrintEmptyLine();
            }

            else
                Output.PrintColoredLine(Constants.AppName, ConsoleColor.Green);

            Output.Print("$ ");
        }
    }
}
