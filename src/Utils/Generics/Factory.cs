using System;
using System.Linq;
using int32.Utils.Extensions;

namespace int32.Utils.Generics
{
    public abstract class Factory<T> where T : class, new()
    {
        public static T Create()
        {
            return Activator.CreateInstance<T>();
        }

        public static T Create(Action<T> constructor)
        {
            var obj = Create();
            constructor(obj);
            return obj;
        }
    }
}
