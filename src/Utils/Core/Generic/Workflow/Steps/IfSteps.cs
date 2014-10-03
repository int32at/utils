using System.Collections.Generic;
using int32.Utils.Core.Extensions;

namespace int32.Utils.Core.Generic.Workflow.Steps
{
    public class IfEqualStep<T> : Step<T>
    {
        internal readonly string Name;
        internal readonly object Compare;
        internal readonly List<BaseStep<T>> IfSteps;
        internal readonly List<BaseStep<T>> ElseSteps;

        public IfEqualStep(string name, object value, IEnumerable<BaseStep<T>> ifSteps, IEnumerable<BaseStep<T>> elseSteps)
            : base(i => { })
        {
            Name = name;
            Compare = value;
            IfSteps = new List<BaseStep<T>>();
            ElseSteps = new List<BaseStep<T>>();

            if (ifSteps != null)
                IfSteps.AddRange(ifSteps);

            if (elseSteps != null)
                ElseSteps.AddRange(elseSteps);
        }

        internal override StepResult<T> Start(T target)
        {
            return StepEngine.StartIfElse(target,
                StepEngine.Equals(target, Name, Compare),
                IfSteps,
                ElseSteps
                );
        }
    }

    public class IfNullStep<T> : IfEqualStep<T>
    {
        public IfNullStep(string name, IEnumerable<BaseStep<T>> ifSteps, IEnumerable<BaseStep<T>> elseSteps)
            : base(name, null, ifSteps, elseSteps) { }

        internal override StepResult<T> Start(T target)
        {
            return StepEngine.StartIfElse(target,
                StepEngine.IsNull(target, Name),
                IfSteps,
                ElseSteps);
        }
    }
}