using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using int32.Utils.Core.Generic.Workflow.Steps;

namespace int32.Utils.Core.Generic.Workflow
{
    public class WorkflowResult<T>
    {
        public T Target { get; internal set; }
        public bool HasErrors { get; internal set; }
        public Exception Exception { get; internal set; }
        public List<BaseStep<T>> CompletedSteps { get; internal set; }
        public List<BaseStep<T>> FailedSteps { get; internal set; }

        public WorkflowResult(T target)
        {
            Target = target;
            CompletedSteps = new List<BaseStep<T>>();
            FailedSteps = new List<BaseStep<T>>();
        }
    }

    public class WorkflowResult : WorkflowResult<object>
    {
        public WorkflowResult()
            : base(null)
        {
        }

        public static WorkflowResult Map(WorkflowResult<object> result)
        {
            return new WorkflowResult
            {
                HasErrors = result.HasErrors,
                Target = result.Target,
                Exception = result.Exception,
                CompletedSteps = result.CompletedSteps,
                FailedSteps = result.FailedSteps
            };
        }
    }
}