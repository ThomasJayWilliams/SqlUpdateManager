using SqlUpdateManager.CLI.Common;
using SqlUpdateManager.CLI.IO;
using System;

namespace SqlUpdateManager.CLI
{
    public class ErrorHanlingMiddleware : IMiddleware
    {
        private readonly ILogger _logger;

        public ErrorHanlingMiddleware(ILogger logger)
        {
            _logger = logger;
        }

        public void Invoke(RequestContext context)
        {
            try
            {
                context.Next();
            }
            catch (CLIException ex)
            {
                _logger.LogError(ex, $"{ex.Code}: {ex.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }
    }
}
