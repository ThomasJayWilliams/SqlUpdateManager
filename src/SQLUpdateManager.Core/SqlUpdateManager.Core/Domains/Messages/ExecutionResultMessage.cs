using System;

namespace SqlUpdateManager.Core
{
    public class ExecutionResultMessage : IMessage
    {
        public bool IsSuccess { get; set; }
        public Exception Error { get; set; }
    }
}
