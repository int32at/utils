using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using int32.Utils.Core.Extensions;

namespace int32.Utils.Core.Generic.Base
{
    public static class RequirementBuilder
    {
        public static Requirement<T>[] Load<T>(string filePath)
        {
            var requirements = new List<Requirement<T>>();
            var data = File.ReadAllText(filePath);
            var collection = data.FromJSON<RequirementExpressions>();

            foreach (var requirement in collection.Requirements)
            {
                var expression = GetExpressionForRequirement<T>(requirement);

                if (expression != null)
                    requirements.Add(new Requirement<T>(expression));
            }

            return requirements.ToArray();
        }

        private static Expression<Func<T, bool>> GetExpressionForRequirement<T>(RequirementExpression requirement)
        {
            var type = typeof(T);
            var parameter = Expression.Parameter(type, "i");
            var property = type.GetProperty(requirement.Property);

            object constant = null;

            if (requirement.Constant.IsNotNull())
            {
                constant = property.PropertyType.IsEnum ?
                    //if its an enum, parse the string to enum
                    Enum.Parse(property.PropertyType, requirement.Constant.ToString()) :
                    //else change the type 
                    Convert.ChangeType(requirement.Constant, property.PropertyType);
            }

            var left = Expression.MakeMemberAccess(parameter, property);
            var right = Expression.Constant(constant);

            var condition = requirement.Check.ParseEnum<ExpressionType>();

            var expression = Expression.MakeBinary(condition, left, right);

            return Expression.Lambda<Func<T, bool>>(expression, parameter);
        }
    }

    internal class RequirementExpressions
    {
        public string Type { get; set; }
        public List<RequirementExpression> Requirements { get; set; }
    }

    internal class RequirementExpression
    {
        public string Property { get; set; }
        public string Check { get; set; }
        public object Constant { get; set; }
    }

    public class Requirement<T>
    {
        public Expression<Func<T, bool>> Check { get; set; }

        public Requirement(Expression<Func<T, bool>> requirement)
        {
            Check = requirement;
        }

        public override string ToString()
        {
            return Check.ToString();
        }
    }
}