using System.Collections.Generic;
using int32.Utils.Core.Generic.Workflow.Steps;

namespace int32.Utils.Core.Generic.Workflow
{
    public class Workflow<T>
    {
        protected T Target;
        public List<BaseStep<T>> Steps { get; internal set; }

        public Workflow(T target, params BaseStep<T>[] steps)
        {
            Target = target;
            Steps = new List<BaseStep<T>>();
            Steps.AddRange(steps);
        }

        public WorkflowResult<T> Start()
        {
            foreach (var step in Steps)
                step.Start(Target);

            return new WorkflowResult<T>(Target);
        }
    }

    public class Workflow : Workflow<object>
    {
        public Workflow(params BaseStep<object>[] steps)
            : base(null, steps)
        {
        }
    }
}