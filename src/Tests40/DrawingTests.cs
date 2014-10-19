using int32.Utils.Core.Extensions;
using int32.Utils.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tests.Samples;

namespace Tests
{
    [TestClass]
    public class DrawingTests
    {
        [TestMethod]
        public void Drawing_Change_Icon_to_ImageSource()
        {
            var imageSource = Data.Default_App_Icon.ToImageSource();
            MakeSure.That(imageSource).IsNot(null);
        }
    }
}
