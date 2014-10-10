using System.Windows;
using int32.Utils.Core.Extensions;
using int32.Utils.Windows.Wpf.Commands;
using int32.Utils.Windows.Wpf.Converters;
using NUnit.Framework;
using Tests.Samples;

namespace Tests
{
    [TestFixture]
    public class WpfTests
    {
        [TestCase]
        public void Wpf_Converters_BooleanToVisibility()
        {
            var converter = new BooleanToVisibilityConverter();

            Assert.AreEqual(Visibility.Visible, converter.Convert(true, null));
            Assert.AreEqual(Visibility.Collapsed, converter.Convert(false, null));
            Assert.AreEqual(Visibility.Visible, converter.Convert(false, "i"));
            Assert.AreEqual(Visibility.Collapsed, converter.Convert(true, "i"));
        }

        [TestCase]
        public void Wpf_BindableBase_BaseViewModel()
        {
            Assert.IsNotNull(new WpfViewModel());
        }

        [TestCase]
        public void Wpf_RelayCommands()
        {
            var count = 0;
            var command = new RelayCommand(o => count++, o => o.IsNotNull());
            Assert.IsTrue(command.CanExecute(count));
            command.Execute(count);
            Assert.AreEqual(1, count);
        }
    }
}
