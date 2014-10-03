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
                throw new Exception(requirement.ToString());

            return item;
        }
    }
}