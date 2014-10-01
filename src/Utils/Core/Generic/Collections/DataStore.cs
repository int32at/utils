using int32.Utils.Core.Extensions;

namespace int32.Utils.Core.Generic.Collections
{
    public class DataStore
    {
        private readonly FluentDictionary<string, object> _data;

        public DataStore()
        {
            _data = new FluentDictionary<string, object>();
        }

        public DataStore Set<T>(string key, T data)
        {
            _data.Add(key, data);
            return this;
        }

        public T Get<T>(string key)
        {
            return _data[key].As<T>();
        }
    }
}
