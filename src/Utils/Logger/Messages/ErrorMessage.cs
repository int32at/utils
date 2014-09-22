using System;
using System.Diagnostics;

namespace int32.Utils.Logger.Messages
{
    public class ErrorMessage : BaseMessage
    {
        public Exception Exception { get; set; }
        public bool EnableDebugging { get; set; }

        public ErrorMessage(Exception ex)
        {
            Level = LogLevel.Error;
            Exception = ex;
        }

        public override string ToString()
        {
            var error = Exception == null ? "NO EXCEPTION PRESENT" : Exception.ToString();

            if (EnableDebugging)
                error += new StackTrace(2).ToString();

            return GetFormattedMessage(error);
        }
    }
}
