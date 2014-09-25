using int32.Utils.Domains;
using int32.Utils.Extensions;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class EnvironmentTests
    {
        [TestCase]
        public void Environment_Create_Object()
        {
            Domain.OnChanged = () => Domain.Current.Switch()
                .Case<Development>(() => Domain.Current.Config.Set("Url", "https://dev"))
                .Case<Production>(() => Domain.Current.Config.Set("Url", "https://prod"))
                .Default(() => Domain.Current.Config.Set("Url", "https://default"));

            Domain.SetTo<Development>();
            Assert.AreEqual("https://dev", Domain.Current.Config["Url"]);

            Domain.SetTo<Production>();
            Assert.AreEqual("https://prod", Domain.Current.Config["Url"]);

            Domain.SetTo<QualityAssurance>();
            Assert.AreEqual("https://default", Domain.Current.Config["Url"]);
        }
    }
}
