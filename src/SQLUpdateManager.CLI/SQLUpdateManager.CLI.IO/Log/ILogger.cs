using System;

namespace SqlUpdateManager.CLI.IO
{
    public interface ILogger
    {
        void LogError(string message);
        void LogError(Exception ex, string message);
        void LogError(Exception ex);
        void LogInfo(string message);
    }
}
