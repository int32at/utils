using System.Windows;
using int32.Utils.Core.Extensions;
using int32.Utils.Tests;
using int32.Utils.Windows.Wpf.Commands;
using int32.Utils.Windows.Wpf.Converters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tests.Samples;

namespace Tests
{
    [TestClass]
    public class WpfTests
    {
        [TestMethod]
        public void Wpf_Converters_BooleanToVisibility()
        {
            var converter = new BooleanToVisibilityConverter();
            MakeSure.That(converter.Convert(true, null)).Is(Visibility.Visible);
            MakeSure.That(converter.Convert(false, null)).Is(Visibility.Collapsed);
            MakeSure.That(converter.Convert(false, "i")).Is(Visibility.Visible);
            MakeSure.That(converter.Convert(true, "i")).Is(Visibility.Collapsed);
        }

        [TestMethod]
        public void Wpf_BindableBase_BaseViewModel()
        {
            MakeSure.That(new WpfViewModel()).IsNot(null);
        }

        [TestMethod]
        public void Wpf_RelayCommands()
        {
            var count = 0;
            var command = new RelayCommand(o => count++, o => o.IsNotNull());
            MakeSure.That(command.CanExecute(count)).Is(true);
            command.Execute(count);
            MakeSure.That(count).Is(1);
        }
    }
}
