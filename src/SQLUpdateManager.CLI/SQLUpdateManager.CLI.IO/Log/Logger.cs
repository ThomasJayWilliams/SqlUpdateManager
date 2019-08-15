using Serilog;
using SQLUpdateManager.CLI.Common;
using System;

namespace SQLUpdateManager.CLI.IO
{
    public class Logger : ILogger
    {
        private readonly Session _session;
        private readonly IOutput _output;

        public Logger(Session session, IOutput output)
        {
            _output = output;
            _session = session;
        }

        public void LogError(string message)
        {
            Log.Error(message);
            _output.Print("error: ");
            _output.PrintLine(message);
        }

        public void LogError(Exception ex, string message)
        {
            Log.Error(ex, message);
            _output.Print("error: ");
            _output.PrintLine(message);
        }

        public void LogError(Exception ex)
        {
            Log.Error(ex, ex.Message);
            _output.Print("error: ");
            _output.PrintLine(ex.Message);
        }

        public void LogInfo(string message)
        {
            Log.Information(message);
            _output.Print("info: ");
            _output.PrintLine(message);
        }
    }
}
