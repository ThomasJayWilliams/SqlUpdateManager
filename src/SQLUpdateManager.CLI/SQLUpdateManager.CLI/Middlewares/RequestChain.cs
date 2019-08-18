using System;
using System.Collections.Generic;

namespace SqlUpdateManager.CLI
{
    public class RequestChain
    {
        private readonly RequestContext _context;

        public RequestContext Context
        {
            get => _context;
        }

        public RequestChain(params IMiddleware[] middlewares)
        {
            if (middlewares == null || middlewares.Length == 0)
                throw new ArgumentException("Call chain cannot be null or empty!");

            var queue = new Queue<IMiddleware>();

            foreach (var mWare in middlewares)
            {
                if (mWare == null)
                    throw new ArgumentNullException("Middleware cannot be null!");

                queue.Enqueue(mWare);
            }

            _context = new RequestContext(queue);
        }

        public void Begin(string command)
        {
            _context.InputCommand = command;
            _context.Next();
        }

    }
}
