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
            Domain.SetTo<Integration>();

            Domain.Current.Switch()
                .Case<Development>(() => Domain.Current.Store.Add("Url", 1))
                .Case<Production>(() => Domain.Current.Store.Add("Url", 2))
                .Default(() => Domain.Current.Store.Add("Url", 3));

            Assert.AreEqual(3, Domain.Current.Store["Url"]);
        }
    }
}
