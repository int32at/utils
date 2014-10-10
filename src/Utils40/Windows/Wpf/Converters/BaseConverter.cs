using System;
using System.Globalization;
using System.Windows.Data;

namespace int32.Utils.Windows.Wpf.Converters
{
    public abstract class BaseConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Convert(value, parameter);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ConvertBack(value, parameter);
        }

        public abstract object Convert(object value, object parameter);
        public abstract object ConvertBack(object value, object parameter);
    }
}
