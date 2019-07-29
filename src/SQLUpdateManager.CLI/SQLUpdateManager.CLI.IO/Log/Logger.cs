using Serilog;
using SQLUpdateManager.CLI.Common;
using System;

namespace SQLUpdateManager.CLI.IO
{
    public class Logger : ILogger
    {
        private readonly Session _session;

        public Logger(Session session)
        {
            _session = session;
        }

        public void LogError(string message)
        {
            Log.Error(message);
            Output.PrintColored("Error ", _session.Theme.ErrorColor);
            Output.PrintLine(message);
        }

        public void LogError(Exception ex, string message)
        {
            Log.Error(ex, message);
            Output.PrintColored("Error ", _session.Theme.ErrorColor);
            Output.PrintLine(message);
        }

        public void LogError(Exception ex)
        {
            Log.Error(ex, ex.Message);
            Output.PrintColored("Error ", _session.Theme.ErrorColor);
            Output.PrintLine(ex.Message);
        }

        public void LogInfo(string message)
        {
            Log.Information(message);
            Output.PrintLine(message);
        }
    }
}
