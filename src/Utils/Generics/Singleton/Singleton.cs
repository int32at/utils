using System;
using int32.Utils.Extensions;

namespace int32.Utils.Generics.Singleton
{
    public class Singleton<T> where T : class
    {
        private static T _instance;

        public static T Instance
        {
            get { return _instance.IfNull(() => _instance = Activator.CreateInstance<T>()); }
        }
    }
}
