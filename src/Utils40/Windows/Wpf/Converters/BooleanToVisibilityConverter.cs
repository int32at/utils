using System.Windows;

namespace int32.Utils.Windows.Wpf.Converters
{
    public class BooleanToVisibilityConverter : BaseConverter
    {
        public override object Convert(object value, object parameter)
        {
            if (parameter is string && ((string)parameter).Equals("i"))
                return value is bool && (bool)value ? Visibility.Collapsed : Visibility.Visible;
            return value is bool && (bool)value ? Visibility.Visible : Visibility.Collapsed;
        }

        public override object ConvertBack(object value, object parameter)
        {
            if (parameter is string && ((string)parameter).Equals("i"))
                return value is Visibility && (Visibility)value == Visibility.Collapsed;
            return value is Visibility && (Visibility)value == Visibility.Visible;
        }
    }
}
