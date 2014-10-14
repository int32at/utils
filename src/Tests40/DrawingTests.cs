using System.Windows.Media;
using int32.Utils.Core.Extensions;
using NUnit.Framework;
using Tests.Samples;

namespace Tests
{
    [TestFixture]
    public class DrawingTests
    {
        [TestCase]
        public void Drawing_Change_Icon_to_ImageSource()
        {
            var imageSource = Data.Default_App_Icon.ToImageSource();
            Assert.That(imageSource.IsNotNull());
        }
    }
}
