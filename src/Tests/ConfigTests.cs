using System;
using int32.Utils.Configuration;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class ConfigTests
    {
        [TestCase]
        public void Config_Create_Object()
        {
            var config = new Config();
            Assert.IsNotNull(config);
        }

        [TestCase]
        public void Config_GetAndSet_Simple()
        {
            const int expected = 3;
            var config = new Config();
            config.Set(new ConfigEntry("MyKey", expected));
            Assert.AreEqual(expected, config.Get<int>("MyKey"));
        }

        [TestCase]
        public void Config_GetAndSet_NotFound()
        {
            const int expected = 3;
            var config = new Config();
            config.Set(new ConfigEntry("MyKey", expected));
            Assert.Throws<ArgumentNullException>(() => config.Get<int>("MyKey1"));
        }

        [TestCase]
        public void Config_GetAndSet_Remove()
        {
            const int expected = 3;
            var config = new Config();
            config.Set(new ConfigEntry("MyKey", expected));
            Assert.AreEqual(expected, config.Get<int>("MyKey"));

            config.Remove("MyKey");

            Assert.Throws<ArgumentNullException>(() => config.Get<int>("MyKey"));
            Assert.Throws<ArgumentNullException>(() => config.Remove("MyKey"));
        }
    }
}
