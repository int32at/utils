using System.Collections.Generic;
using int32.Utils.Extensions;

namespace int32.Utils.Generics
{
    public abstract class BaseHandler<T>
    {
        private static readonly List<T> Handlers = new List<T>();

        public static void Add(T handle)
        {
            if (!Handlers.Contains(handle))
                Handlers.Add(handle);
        }

        public static void Add(params T[] handler)
        {
            handler.ForEach(Add);
        }

        public static THandler Get<THandler>()
        {
            return Handlers.Find(i => i.GetType() == typeof(THandler)).As<THandler>();
        }
    }
}
