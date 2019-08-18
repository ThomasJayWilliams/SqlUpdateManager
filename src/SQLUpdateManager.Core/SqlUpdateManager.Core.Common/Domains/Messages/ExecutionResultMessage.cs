using System;

namespace SqlUpdateManager.Core.Common
{
    public class ExecutionResultMessage : IMessage
    {
        public bool IsSuccess { get; set; }
        public Exception Error { get; set; }
    }
}
