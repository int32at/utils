using System;
using System.Linq;
using System.Linq.Expressions;

namespace int32.Utils.Core.Generic.Base
{
    public static class Require
    {
        public static T That<T>(T item, params Expression<Func<T, bool>>[] requirements)
        {
            foreach (var requirement in requirements.Where(requirement => !requirement.Compile()(item)))
                throw new RequirementNotMetException(requirement);

            return item;
        }

        public static T That<T>(T item, params Requirement<T>[] requirements)
        {
            foreach (var requirement in requirements.Where(requirement => !requirement.Check.Compile()(item)))
                throw new RequirementNotMetException(requirement);

            return item;
        }
    }

    public class RequirementNotMetException : Exception
    {
        public new string Message { get; internal set; }

        internal RequirementNotMetException(object requirement)
        {
            Message = string.Format(Resources.Utils.Require_ThrowException, requirement);
        }
    }
}