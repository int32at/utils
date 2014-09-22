namespace int32.Utils.Logger.Messages
{
    public class InfoMessage : BaseMessage
    {
        public string Message { get; set; }

        public InfoMessage(string message)
        {
            Level = LogLevel.Info;
            Message = message;
        }

        public override string ToString()
        {
            return GetFormattedMessage(Message);
        }
    }
}
