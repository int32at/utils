using System;
using int32.Utils.Logger;
using NUnit.Framework;
using Tests.Samples;

namespace Tests
{
    [TestFixture]
    public class LoggerTests
    {
        [TestCase]
        public void Logger_Create_Object()
        {
            using (var logger = new Logger())
            {
                Assert.IsNotNull(logger);
            }
        }

        [TestCase]
        public void Logger_Create_MockLogger()
        {
            using (var logger = new Logger(new MockLogger()))
            {
                Assert.IsNotNull(logger);
                Assert.IsNotNull(logger.Config);

                logger.Warn("WarnMessage");
                logger.Info("InfoMessage");
                logger.Error(new ArgumentOutOfRangeException());
            }
        }
    }
}
