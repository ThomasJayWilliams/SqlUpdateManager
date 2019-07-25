using Serilog;
using SQLUpdateManager.CLI.Common;
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
                Log.Logger.Error(ex, $"{ex.Code}: {ex.Message}");
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
            }
        }
    }
}
