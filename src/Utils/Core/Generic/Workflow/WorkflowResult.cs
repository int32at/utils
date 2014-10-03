namespace int32.Utils.Core.Generic.Workflow
{
    public class WorkflowResult<T>
    {
        public T Target { get; internal set; }
        public bool HasErrors { get; internal set; }

        public WorkflowResult(T target)
        {
            Target = target;
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
                Target = result.Target
            };
        }
    }
}