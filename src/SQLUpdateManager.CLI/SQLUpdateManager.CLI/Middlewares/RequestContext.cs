using System.Collections.Generic;

namespace SqlUpdateManager.CLI
{
    public class RequestContext
    {
        public string InputCommand { get; set; }

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
