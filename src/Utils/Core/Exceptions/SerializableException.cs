using System;

namespace int32.Utils.Core.Exceptions
{
    [Serializable]
    public class SerializableException
    {
        public DateTime At { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }

        public SerializableException()
        {
            At = DateTime.Now;
        }

        public SerializableException(Exception ex)
            : this()
        {
            Message = ex.Message;
            StackTrace = ex.StackTrace;
        }

        public SerializableException(string message)
            : this()
        {
            Message = message;
        }

        public override string ToString()
        {
            return Message + StackTrace;
        }
    }
}