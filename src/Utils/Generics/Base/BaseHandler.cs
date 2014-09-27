using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using int32.Utils.Extensions;

namespace int32.Utils.Generics.Base
{
    public abstract class BaseHandler<T> where T : class
    {
        private static readonly List<T> Handlers = new List<T>();

        public static T Add(T handle)
        {
            return Find(handle.GetType())
                .IfNull(() =>
                {
                    Handlers.Add(handle);
                    return handle;
                })
                .IfNotNull(i => i);
        }

        public static IEnumerable<T> Add(params T[] handler)
        {
            return handler.Select(Add).ToList();
        }

        public static THandler Get<THandler>() where THandler : T
        {
            return Find(typeof(THandler)).As<THandler>();
        }

        private static T Find(Type type)
        {
            return Handlers.FirstOrDefault(i => i.GetType() == type);
        }
    }
}