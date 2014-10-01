using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using int32.Utils.Core.Extensions;

namespace int32.Utils.Core.Generic.Base
{
    public static class Require
    {
        public static T That<T>(T item, params Expression<Func<T, bool>>[] requirements)
        {
            foreach (var requirement in requirements.Where(requirement => !requirement.Compile()(item)))
                throw new Exception(requirement.ToString());

            return item;
        }
    }

    public class RequirementNotCompliedException : Exception
    {
        public RequirementNotCompliedException(string message) : base(message) { }

        public RequirementNotCompliedException(string message, Exception innerException) : base(message, innerException) { }

        public RequirementNotCompliedException(LambdaExpression lex)
        {
        }
    }
}