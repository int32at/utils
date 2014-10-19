using int32.Utils.Core.Logger;
using int32.Utils.Core.Logger.Contracts;
using int32.Utils.Core.Logger.Messages;
using int32.Utils.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Samples
{
    public class MockLogger : ILogger
    {
        public void Dispose()
        {
            MakeSure.That(true).Is(true);
        }

        public void Info(InfoMessage msg)
        {
            Assert.IsNotNull(msg);
            Assert.AreEqual("InfoMessage", msg.Message);
            Assert.AreEqual(LogLevel.Info, msg.Level);
        }

        public void Warn(WarnMessage msg)
        {
            Assert.IsNotNull(msg);
            Assert.AreEqual("WarnMessage", msg.Message);
            Assert.AreEqual(LogLevel.Warn, msg.Level);
        }

        public void Error(ErrorMessage msg)
        {
            Assert.IsNotNull(msg);
            Assert.AreEqual(LogLevel.Error, msg.Level);
            Assert.IsNotNull(msg.ToString());
        }
    }
}
