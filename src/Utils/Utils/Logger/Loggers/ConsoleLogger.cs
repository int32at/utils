using System;
using int32.Utils.Logger.Contracts;
using int32.Utils.Logger.Messages;

namespace int32.Utils.Logger.Loggers
{
    public class ConsoleLogger : ILogger
    {
        public void Dispose()
        {
        }

        public void Info(InfoMessage msg)
        {
            Console.WriteLine(msg);
        }

        public void Warn(WarnMessage msg)
        {
            Console.WriteLine(msg);
        }

        public void Error(ErrorMessage msg)
        {
            Console.WriteLine(msg);
        }
    }
}
