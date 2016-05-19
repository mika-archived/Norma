using System;
using System.Globalization;
using System.Windows.Data;

namespace Norma.Converters
{
    internal class EnumToBooleanConverter : IValueConverter
    {
        #region Implementation of IValueConverter

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.ToString() == parameter as string;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var b = value as bool?;
            if (!b.HasValue)
                return null;
            return b.Value ? Enum.Parse(targetType, parameter as string ?? "", true) : null;
        }

        #endregion
    }
}