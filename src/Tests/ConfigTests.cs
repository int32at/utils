using System;
using int32.Utils.Core.Configuration;
using int32.Utils.Core.Extensions;
using int32.Utils.Tests;
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
            MakeSure.That(config).IsNot(null);
        }

        [TestCase]
        public void Config_GetAndSet_Simple()
        {
            const int expected = 3;

            var config = new Config();
            config.Set(new ConfigEntry("MyKey", expected));

            MakeSure.That(config.Get<int>("MyKey")).Is(expected);
            MakeSure.That(config["MyKey"].As<int>()).Is(expected);
        }

        [TestCase]
        public void Config_GetAndSet_NotFound()
        {
            const int expected = 3;

            var config = new Config();
            config.Set(new ConfigEntry("MyKey", expected));

            MakeSure.That(() => config.Get<int>("MyKey1")).Throws<ArgumentNullException>();
        }

        [TestCase]
        public void Config_GetAndSet_Remove()
        {
            const int expected = 3;

            var config = new Config();
            config.Set(new ConfigEntry("MyKey", expected));
            MakeSure.That(config.Get<int>("MyKey")).Is(expected);

            config.Remove("MyKey");

            MakeSure.That(() => config.Get<int>("MyKey")).Throws<ArgumentNullException>();
        }

        [TestCase]
        public void Config_LoadFrom_ConfigManager()
        {
            //load the app config file
            var cfg = Config.Create();
            MakeSure.That(cfg.Count).IsGreaterThan(0);
        }
    }
}
