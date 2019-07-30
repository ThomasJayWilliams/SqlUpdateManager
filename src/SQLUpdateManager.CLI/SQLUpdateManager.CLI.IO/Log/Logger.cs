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
            _output.PrintColored("error: ", _session.Theme.ErrorColor);
            _output.PrintColoredLine(message, _session.Theme.TextColor);
        }

        public void LogError(Exception ex, string message)
        {
            Log.Error(ex, message);
            _output.PrintColored("error: ", _session.Theme.ErrorColor);
            _output.PrintColoredLine(message, _session.Theme.TextColor);
        }

        public void LogError(Exception ex)
        {
            Log.Error(ex, ex.Message);
            _output.PrintColored("error: ", _session.Theme.ErrorColor);
            _output.PrintColoredLine(ex.Message, _session.Theme.TextColor);
        }

        public void LogInfo(string message)
        {
            Log.Information(message);
            _output.PrintColored("info: ", _session.Theme.InfoColor);
            _output.PrintColoredLine(message, _session.Theme.TextColor);
        }
    }
}
