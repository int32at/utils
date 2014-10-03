namespace int32.Utils.Core.Generic.Workflow
{
    public class Engine
    {
        public WorkflowResult Start(Workflow workflow)
        {
            var result = workflow.Start();
            return WorkflowResult.Map(result);
        }

        public WorkflowResult<T> Start<T>(Workflow<T> workflow)
        {
            return workflow.Start();
        }
    }
}
