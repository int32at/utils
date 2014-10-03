using System;
using System.Collections.Generic;
using int32.Utils.Core.Generic.Engine.Workflow.Steps;

namespace int32.Utils.Core.Generic.Engine.Workflow
{
    public abstract class BaseWorkflowResult
    {
        public object Target { get; internal set; }
        public bool HasErrors { get; internal set; }
        public Exception Exception { get; internal set; }
        public List<BaseWorkflowStep> CompletedSteps { get; internal set; }
        public List<BaseWorkflowStep> FailedSteps { get; internal set; }

        protected BaseWorkflowResult()
        {
            CompletedSteps = new List<BaseWorkflowStep>();
            FailedSteps = new List<BaseWorkflowStep>();
        }
    }

    public class WorkflowResult : BaseWorkflowResult { }

    public class WorkflowResult<T> : BaseWorkflowResult
    {
        public new T Target { get; internal set; }
    }
}
