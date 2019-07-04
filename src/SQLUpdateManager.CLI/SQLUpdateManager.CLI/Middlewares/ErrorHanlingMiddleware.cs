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
            catch (Exception)
            {
                // TODO: Catch all kinds of exceptions, then log it and print output got user.
            }
        }
    }
}
