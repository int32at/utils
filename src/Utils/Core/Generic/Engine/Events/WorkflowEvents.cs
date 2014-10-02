using System;

namespace int32.Utils.Core.Generic.Engine.Events
{
    public class WorkflowCompletedEventArgs : EventArgs
    {
        public TimeSpan ExecutionTime { get; set; }
    }
}
