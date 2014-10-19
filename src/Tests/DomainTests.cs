using int32.Utils.Core.Domain;
using int32.Utils.Core.Extensions;
using int32.Utils.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class EnvironmentTests 
    {
        [TestMethod]
        public void Domain_LoadFrom_Config()
        {
            Domain.Current.Config.Load(); //load from app config
            MakeSure.That(Domain.Current.Config["Url"]).Is("http://dev");
        }

        [TestMethod]
        public void Domain_LoadFromConfig_Switch()
        {
            Domain.OnChanged = () => Domain.Current.Switch()
                .Case<Production>(() => Domain.Current.Config.Set("Url", "http://prod"))
                .Default(() => Domain.Current.Config.Load());

            Domain.SetTo<Development>();

            MakeSure.That(Domain.Current.Config["Url"]).Is("http://dev");

            Domain.SetTo<Production>();
            MakeSure.That(Domain.Current.Config["Url"]).Is("http://prod");
        }
    }
}
