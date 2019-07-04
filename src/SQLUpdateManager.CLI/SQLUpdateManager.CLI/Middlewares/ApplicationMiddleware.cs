using SQLUpdateManager.Core.Common;

namespace SQLUpdateManager.CLI
{
    public class ApplicationMiddleware : IMiddleware
    {
        private readonly ISerializer _serializer;

        public ApplicationMiddleware(ISerializer serializer)
        {
            _serializer = serializer;
        }

        public void Invoke(RequestContext context)
        {
            context.Next();
        }
    }
}
