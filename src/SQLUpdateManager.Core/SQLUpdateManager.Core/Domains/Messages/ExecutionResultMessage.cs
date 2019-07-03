using System;

namespace SQLUpdateManager.Core.Domains
{
    public class ExecutionResultMessage : IMessage
    {
        public bool IsSuccess { get; set; }
        public Exception Error { get; set; }
    }
}
