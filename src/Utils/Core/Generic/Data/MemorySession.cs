using System;
using System.Collections.Generic;
using System.Linq;
using int32.Utils.Core.Extensions;

namespace int32.Utils.Core.Generic.Data
{
    public class MemorySession : IDisposable
    {
        private List<object> _databases;

        public MemorySession()
        {
            _databases = new List<object>();
        }


        public MemoryDatabase<T> Database<T>()
        {
            var type = typeof(MemoryDatabase<T>);

            var db = _databases.FirstOrDefault(i => i.GetType() == type);

            if (!db.IsNull()) return db.As<MemoryDatabase<T>>();

            db = new MemoryDatabase<T>();
            _databases.Add(db);

            return db.As<MemoryDatabase<T>>();
        }

        public void Dispose()
        {
            _databases = null;
        }
    }
}
