﻿using Serilog;
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
                Log.Error($"{ex.Code}: {ex.Message}");
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, ex.Message);
            }
        }
    }
}
