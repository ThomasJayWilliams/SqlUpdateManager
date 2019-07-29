using Serilog;
using System;

namespace SQLUpdateManager.CLI.IO
{
    public static class SerilogLogger
    {
        public static void LogError(string message) =>
            Log.Error(message);

        public static void LogError(Exception ex, string message) =>
            Log.Error(ex, message);

        public static void LogError(Exception ex) =>
            Log.Error(ex, ex.Message);

        public static void LogInfo(string message) =>
            Log.Information(message);
    }
}
