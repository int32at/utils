using System.IO;
using int32.Utils.Logger.Contracts;
using int32.Utils.Logger.Messages;

namespace int32.Utils.Logger.Loggers
{
    public class FileLogger : ILogger
    {
        private readonly string _filePath;

        public FileLogger(string filePath)
        {
            _filePath = filePath;
        }

        public void Dispose()
        {
        }

        public void Info(InfoMessage msg)
        {
            Log(msg);
        }

        public void Warn(WarnMessage msg)
        {
            Log(msg);
        }

        public void Error(ErrorMessage msg)
        {
            Log(msg);
        }

        private void Log(BaseMessage msg)
        {
            File.AppendAllText(_filePath, msg + System.Environment.NewLine);
        }
    }
}
