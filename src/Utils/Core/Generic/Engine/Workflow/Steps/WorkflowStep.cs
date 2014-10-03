using System;

namespace int32.Utils.Core.Generic.Engine.Workflow.Steps
{
    public abstract class BaseWorkflowStep
    {
        public string Title { get; internal set; }
        public Delegate Action { get; internal set; }

        protected BaseWorkflowStep(Delegate action)
        {
            Action = action;
        }
    }

    public class WorkflowStep : BaseWorkflowStep
    {
        public WorkflowStep(Action action) : base(action) { }
    }

    public class WorkflowStep<T> : BaseWorkflowStep
    {
        public WorkflowStep(Action<T> action) : base(action) { }

        public WorkflowStep(Func<T, T> action) : base(action) { }
    }
}
