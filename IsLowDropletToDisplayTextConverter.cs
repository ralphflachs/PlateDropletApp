using System;
using System.Globalization;
using System.Windows.Data;

namespace PlateDropletApp.Converters
{
    public class IsLowDropletToDisplayTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isLowDroplet = (bool)value;
            return isLowDroplet ? "L" : "n";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
