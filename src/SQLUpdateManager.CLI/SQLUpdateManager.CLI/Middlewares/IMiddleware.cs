﻿namespace SQLUpdateManager.CLI
{
    public interface IMiddleware
    {
        void Invoke(RequestContext context);
    }
}
