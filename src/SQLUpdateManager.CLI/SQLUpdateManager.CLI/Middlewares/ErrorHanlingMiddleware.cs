using SQLUpdateManager.CLI.Common;
using SQLUpdateManager.CLI.IO;
using System;

namespace SQLUpdateManager.CLI
{
    public class ErrorHanlingMiddleware : IMiddleware
    {
        public void Invoke(RequestContext context)
        {
            try
            {
                context.Next();
            }
            catch (CLIException ex)
            {
                OutputHandler.PrintColoredLine($"{Convert.ToInt32(ex.Code)} {ex.Code}", ConsoleColor.Red);
                OutputHandler.PrintColoredLine($"{ex.Message}", ConsoleColor.Red);
            }
            catch (Exception ex)
            {
                OutputHandler.PrintColoredLine(ex.Message, ConsoleColor.Red);
            }
        }
    }
}
