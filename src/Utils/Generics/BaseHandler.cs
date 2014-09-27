using System.Collections.Generic;
using System.Linq;
using int32.Utils.Extensions;

namespace int32.Utils.Generics
{
    public abstract class BaseHandler<T>
    {
        private static readonly List<T> Handlers = new List<T>();

        public static T Add(T handle)
        {
            if (Handlers.Contains(handle)) return Get<T>();
            Handlers.Add(handle);

            return handle;
        }

        public static IEnumerable<T> Add(params T[] handler)
        {
            return handler.Select(Add).ToList();
        }

        public static THandler Get<THandler>() where THandler : T
        {
            return Handlers.Find(i => i.GetType() == typeof(THandler)).As<THandler>();
        }
    }
}
