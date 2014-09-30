using System;
using System.Collections;
using System.Collections.Generic;

namespace int32.Utils.Core.Exceptions
{
    [Serializable]
    public class SerializableException
    {
        public KeyValuePair<object, object>[] Data { get; private set; }
        public SerializableException InnerException { get; private set; }
        public string Message { get; private set; }
        public string Source { get; private set; }
        public string StackTrace { get; private set; }


        public DateTime At { get; set; }

        public SerializableException(Exception ex)
        {
            if (ex == null) return;
            SetDataField(Data);
            Message = ex.Message;
            Source = ex.Source;
            StackTrace = ex.StackTrace;
            InnerException = new SerializableException(ex.InnerException);
        }

        private void SetDataField(ICollection collection)
        {
            Data = new KeyValuePair<object, object>[0];

            if (null != collection)
                collection.CopyTo(Data, 0);
        }
    }
}