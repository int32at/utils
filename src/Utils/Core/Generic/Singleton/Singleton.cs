using int32.Utils.Core.Extensions;
using int32.Utils.Core.Generic.Factory;

namespace int32.Utils.Core.Generic.Singleton
{
    public class Singleton<T> where T : class
    {
        private static T _instance;

        public static T Instance
        {
            get { return _instance.IfNull(() => _instance = Factory<T>.Create()); }
        }
    }
}
