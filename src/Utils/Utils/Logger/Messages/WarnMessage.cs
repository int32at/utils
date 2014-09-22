namespace int32.Utils.Logger.Messages
{
    public class WarnMessage : BaseMessage
    {
        public string Message { get; set; }

        public WarnMessage(string message)
        {
            Level = LogLevel.Warn;
            Message = message;
        }

        public override string ToString()
        {
            return GetFormattedMessage(Message);
        }
    }
}