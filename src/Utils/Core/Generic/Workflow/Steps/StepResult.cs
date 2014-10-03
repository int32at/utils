namespace int32.Utils.Core.Generic.Workflow.Steps
{
    public class StepResult<T>
    {
        public T Result { get; internal set; }

        public StepResult(T item)
        {
            Result = item;
        }
    }

    public class StepResult : StepResult<object>
    {
        public StepResult(object item) : base(item) { }
    }
}
