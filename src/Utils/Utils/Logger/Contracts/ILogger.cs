using System;
using int32.Utils.Logger.Messages;

namespace int32.Utils.Logger.Contracts
{
    public interface ILogger : IDisposable
    {
        void Info(InfoMessage msg);
        void Warn(WarnMessage msg);
        void Error(ErrorMessage msg);
    }
}
