using System;

namespace int32.Utils.Core.Generic.Engine.Events
{
    public class StepCompletedEventArgs : EventArgs
    {
        public Step Step { get; private set; }

        public StepCompletedEventArgs(Step step)
        {
            Step = step;
        }
    }

    public class StepErrorEventArgs : StepCompletedEventArgs
    {
        public StepErrorEventArgs(Step step)
            : base(step)
        {
        }
    }
}
