using int32.Utils.Core.Domain;
using int32.Utils.Core.Extensions;
using int32.Utils.Core.Generic.ViewEngine;
using int32.Utils.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class ViewEngineTests 
    {
        [TestMethod]
        public void ViewEngine_Dynamic_String()
        {
            var result = ViewEngine.Instance.Render("test", null);

            MakeSure.That(result).Is("test");
        }
    }
}
