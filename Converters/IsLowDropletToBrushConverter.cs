using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace PlateDropletApp.Converters
{
    public class IsLowDropletToBrushConverter : IValueConverter
    {
        private static readonly Brush LowDropletBrush = Brushes.Gray;
        private static readonly Brush NormalBrush = Brushes.White;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isLowDroplet = (bool)value;
            return isLowDroplet ? LowDropletBrush : NormalBrush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
