using System;
using int32.Utils.Core.Extensions;

namespace int32.Utils.Core.Generic.Factory
{
    public abstract class Factory<T> where T : class
    {
        public static T Create()
        {
            return Activator.CreateInstance<T>();
        }

        public static T Create(Action<T> constructor)
        {
            var obj = Create();
            constructor.Execute(obj);
            return obj;
        }
    }
}
