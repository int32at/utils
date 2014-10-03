using System;
using System.Collections.Generic;
using System.Configuration;
using int32.Utils.Core.Extensions;
using int32.Utils.Core.Generic.Engine.Workflow.Steps;

namespace int32.Utils.Core.Generic.Engine.Workflow
{
    public abstract class BaseWorkflow
    {
        internal object Target;
        public string Title { get; set; }
        public List<BaseWorkflowStep> Steps { get; internal set; }

        protected BaseWorkflow(params BaseWorkflowStep[] steps)
        {
            Steps = new List<BaseWorkflowStep>();
            Steps.AddRange(steps);
        }

        internal BaseWorkflowResult Start<T>()
        {
            BaseWorkflowResult result;

            if (Target.IsNull())
                result = new WorkflowResult();
            else
                result = new WorkflowResult<T>();

            foreach (var step in Steps)
            {
                try
                {
                    if (Target.IsNull())
                        step.Action.DynamicInvoke();
                    else
                    {
                        var returnVal = step.Action.DynamicInvoke(Target);

                        if (returnVal.IsNotNull())
                            Target = returnVal;
                    }

                    result.CompletedSteps.Add(step);
                }
                catch (Exception ex)
                {
                    result.Exception = ex.InnerException ?? ex;
                    result.HasErrors = true;
                    result.FailedSteps.Add(step);
                    break;
                }
                finally
                {
                    result.Target = Target;
                }
            }

            return result;
        }
    }

    public class Workflow : BaseWorkflow
    {
        public Workflow(params BaseWorkflowStep[] steps) : base(steps) { }
    }

    public class Workflow<T> : BaseWorkflow
    {
        public Workflow(T target, params BaseWorkflowStep[] steps)
            : base(steps)
        {
            Target = target;
        }
    }
}
