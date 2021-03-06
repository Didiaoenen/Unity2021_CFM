using System;

namespace Mvvm
{
    public interface IValueConverter
    {
        object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture);

        object ConvertBack(object value, Type target, object parameter, System.Globalization.CultureInfo culture);
    }
}
