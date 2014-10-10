using System;

namespace int32.Utils.Core.Logger.Messages
{
    public abstract class BaseMessage
    {
        public LogLevel Level { get; set; }

        public DateTime At { get; set; }

        public string Format { get; set; }

        protected BaseMessage()
        {
            Level = LogLevel.Debug;
            At = DateTime.Now;
        }

        protected string GetFormattedMessage(string additional)
        {
            return string.Format(Format, At, Level.ToString().ToUpper(), additional);
        }
    }
}
