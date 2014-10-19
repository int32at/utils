using System;
using System.IO;
using int32.Utils.Core.Logger;
using int32.Utils.Core.Logger.Contracts;
using int32.Utils.Core.Logger.Loggers;
using int32.Utils.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tests.Samples;

namespace Tests
{
    [TestClass]
    public class LoggerTests
    {
        [TestMethod]
        public void Logger_Create_Object()
        {
            using (var logger = new Logger())
            {
                MakeSure.That(logger).IsNot(null);
            }
        }

        [TestMethod]
        public void Logger_Create_MockLogger()
        {
            TestLogger(new MockLogger());
        }

        [TestMethod]
        public void Logger_Create_ConsoleLogger()
        {
            TestLogger(new ConsoleLogger());
        }

        [TestMethod]
        public void Logger_Create_FileLogger()
        {
            const string path = @"log.txt";
            TestLogger(new FileLogger(path));

            MakeSure.That(File.Exists(path)).Is(true);
            File.Delete(path);
        }

        private void TestLogger(ILogger log)
        {
            using (var logger = new Logger(log))
            {
                MakeSure.That(logger).IsNot(null);
                MakeSure.That(logger.Config).IsNot(null);

                //found no better way to do this
                MakeSure.That(() =>
                {
                    logger.Warn("WarnMessage");
                    logger.Warn("{0}{1}", "Warn", "Message");
                    logger.Info("InfoMessage");
                    logger.Info("{0}{1}", "Info", "Message");
                    logger.Error(new ArgumentOutOfRangeException());
                }).DoesNotThrow();
            }
        }
    }
}
