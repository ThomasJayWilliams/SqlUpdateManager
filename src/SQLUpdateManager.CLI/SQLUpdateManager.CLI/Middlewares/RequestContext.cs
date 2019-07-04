using System.Collections.Generic;

namespace SQLUpdateManager.CLI
{
    public class RequestContext
    {
        private Queue<IMiddleware> _callChain;

        public RequestContext(Queue<IMiddleware> middlewares)
        {
            _callChain = middlewares;
        }

        public void Next()
        {
            if (!_callChain.TryDequeue(out var middleware))
                return;

            middleware.Invoke(this);
        }
    }
}
