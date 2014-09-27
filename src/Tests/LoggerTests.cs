using System;
using System.IO;
using int32.Utils.Logger;
using int32.Utils.Logger.Contracts;
using int32.Utils.Logger.Loggers;
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
            TestLogger(new MockLogger());
        }

        [TestCase]
        public void Logger_Create_ConsoleLogger()
        {
            TestLogger(new ConsoleLogger());
        }

        [TestCase]
        public void Logger_Create_FileLogger()
        {
            const string path = @"log.txt";
            TestLogger(new FileLogger(path));

            Assert.IsTrue(File.Exists(path));
            File.Delete(path);
        }

        private void TestLogger(ILogger log)
        {
            using (var logger = new Logger(log))
            {
                Assert.IsNotNull(logger);
                Assert.IsNotNull(logger.Config);

                //found no better way to do this
                Assert.DoesNotThrow(() =>
                {
                    logger.Warn("WarnMessage");
                    logger.Warn("{0}{1}", "Warn", "Message");
                    logger.Info("InfoMessage");
                    logger.Info("{0}{1}", "Info", "Message");
                    logger.Error(new ArgumentOutOfRangeException());
                });
            }
        }
    }
}
