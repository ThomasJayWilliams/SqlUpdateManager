using SQLUpdateManager.Core.Common;

namespace SQLUpdateManager.CLI
{
    public class ApplicationMiddleware : IMiddleware
    {
        private readonly ISerializer _serializer;
        private readonly ICommandParser _parser;

        public ApplicationMiddleware(ISerializer serializer, ICommandParser parser)
        {
            _serializer = serializer;
            _parser = parser;
        }

        public void Invoke(RequestContext context)
        {
            var command = _parser.Parse(context.InputCommand);
            command.ValidateAndRun();
            context.Next();
        }
    }
}
