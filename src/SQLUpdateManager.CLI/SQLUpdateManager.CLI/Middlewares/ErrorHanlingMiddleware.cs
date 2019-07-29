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
                SerilogLogger.LogError(ex, $"{ex.Code}: {ex.Message}");
            }
            catch (Exception ex)
            {
                SerilogLogger.LogError(ex, ex.Message);
            }
        }
    }
}
