using System;
using System.Globalization;
using System.Windows.Data;

using Norma.Eta.Properties;

namespace Norma.Eta.Converters
{
    public class YesNoToBooleanConverter : IValueConverter
    {
        #region Implementation of IValueConverter

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var b = value as bool?;
            if (!b.HasValue)
                return null;
            return b.Value ? Resources.Yes : Resources.No;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var str = value as string;
            if (str == null)
                return null;
            return str == Resources.Yes;
        }

        #endregion
    }
}