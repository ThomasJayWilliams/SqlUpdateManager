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
                OutputHandler.PrintColored(Constants.AppName, ConsoleColor.Green);

                OutputHandler.Print(" ");
                OutputHandler.PrintColored(
                    Session.Current.ConnectedServer.Name.Length > 10 ?
                        Session.Current.ConnectedServer.Name.Substring(0, 10) :
                        Session.Current.ConnectedServer.Name,
                    ConsoleColor.Yellow);

                if (Session.Current.UsedDatabase != null)
                {
                    OutputHandler.Print(" ");
                    OutputHandler.PrintColoredLine(
                        Session.Current.UsedDatabase.Name.Length > 10 ?
                            Session.Current.UsedDatabase.Name.Substring(0, 10) :
                            Session.Current.UsedDatabase.Name,
                        ConsoleColor.Cyan);
                }

                else
                    OutputHandler.PrintLine("");
            }

            else
                OutputHandler.PrintColoredLine(Constants.AppName, ConsoleColor.Green);

            OutputHandler.Print("$ ");
        }
    }
}
