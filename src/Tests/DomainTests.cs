using int32.Utils.Domains;
using int32.Utils.Extensions;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class EnvironmentTests
    {
        [TestCase]
        public void Domain_LoadFrom_Config()
        {
            Domain.Current.Config.Load(); //load from app config
            Assert.AreEqual("http://dev", Domain.Current.Config["Url"]);
        }

        [TestCase]
        public void Domain_LoadFromConfig_Switch()
        {
            Domain.OnChanged = () => Domain.Current.Switch()
                .Case<Production>(() => Domain.Current.Config.Set("Url", "http://prod"))
                .Default(() => Domain.Current.Config.Load());

            Domain.SetTo<Development>();

            Assert.AreEqual("http://dev", Domain.Current.Config["Url"]);

            Domain.SetTo<Production>();
            Assert.AreEqual("http://prod", Domain.Current.Config["Url"]);
        }
    }
}
