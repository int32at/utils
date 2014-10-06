using System;
using System.Collections.Generic;
using System.Linq;
using int32.Utils.Core.Extensions;

namespace int32.Utils.Core.Generic.Data
{
    public class FlatSession : IDisposable
    {
        private List<object> _databases;

        public FlatSession()
        {
            _databases = new List<object>();
        }

        public FlatDatabase<T> Database<T>()
        {
            var name = typeof(T).Name;
            return Database<T>(name);
        }

        public FlatDatabase<T> Database<T>(string path)
        {
            var type = typeof(FlatDatabase<T>);


            var db = _databases.FirstOrDefault(i => i.GetType() == type);

            if (db.IsNull())
            {
                db = new FlatDatabase<T>(path);
                _databases.Add(db);
            }

            return db.As<FlatDatabase<T>>();
        }

        public void Dispose()
        {
            _databases = null;
        }
    }
}