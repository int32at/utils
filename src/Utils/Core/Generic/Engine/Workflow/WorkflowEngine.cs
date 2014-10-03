using System;
using int32.Utils.Core.Extensions;

namespace int32.Utils.Core.Generic.Engine.Workflow
{
    public class WorkflowEngine
    {
        public WorkflowResult Start(Workflow workflow)
        {
            return (WorkflowResult)workflow.Start<Object>();
        }

        public WorkflowResult<T> Start<T>(Workflow<T> workflow)
        {
            var result = workflow.Start<T>();

            //by casting back to the generic result 
            //it looses the Target object, so we have to manually reset it...

            var tmp = (WorkflowResult<T>)result;
            tmp.Target = result.Target.As<T>();
            return tmp;
        }
    }
}