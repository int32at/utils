using System;
using int32.Utils.Core.Extensions;

namespace int32.Utils.Core.Generic.Workflow.Steps
{
    public abstract class BaseStep<T>
    {
        public Delegate Action { get; internal set; }

        protected BaseStep(Delegate action)
        {
            Action = action;
        }

        internal abstract StepResult<T> Start(T target);

        internal T Execute(params object[] parameters)
        {
            if (Action != null)
            {
                var result = Action.Execute(parameters);

                if (result.IsNotNull())
                    return result.As<T>();
            }

            return default(T);
        }
    }

    public class Step : BaseStep<object>
    {
        public Step(Action action)
            : base(action)
        {
        }

        internal override StepResult<object> Start(object target)
        {
            //ignore target, a normal step is just an action, so no return type!
            return new StepResult(Execute());
        }
    }

    public class Step<T> : BaseStep<T>
    {
        public Step(Func<T, T> action) : base(action) { }
        public Step(Action<T> action) : base(action) { }

        internal override StepResult<T> Start(T target)
        {
            //inject the target, since we want to reuse that!
            return new StepResult<T>(Execute(target));
        }
    }
}