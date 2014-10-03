//using System.Collections.Generic;
//using int32.Utils.Core.Extensions;

//namespace int32.Utils.Core.Generic.Workflow.Steps
//{

//    internal static class StepEngine
//    {
//        internal static StepResult<T> StartIfElse<T>(T target, bool condition, IEnumerable<BaseStep<T>> ifSteps,
//            IEnumerable<BaseStep<T>> elseSteps)
//        {
//            if (condition)
//                ifSteps.ForEach(i => i.Start(target));

//            else
//                elseSteps.ForEach(i => i.Start(target));

//            return new StepResult<T>(target);
//        }

//        internal static bool Equals(object source, string propName, object target)
//        {
//            var prop = source.Get(propName);
//            return prop != null && prop.Equals(target);
//        }

//        internal static bool IsNull(object source, string propName)
//        {
//            var prop = source.Get(propName);
//            return prop.IsNull();
//        }
//    }
//}
