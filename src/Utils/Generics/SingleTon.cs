using int32.Utils.Extensions;

namespace int32.Utils.Generics
{
    public class Singleton<T> where T : new()
    {
        private static T _instance;

        public static T Instance
        {
            get { return _instance.IfNull(() => _instance = new T()); }
        }
    }
}
