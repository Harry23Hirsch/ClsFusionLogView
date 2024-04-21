using System;
using System.Globalization;
using System.Windows.Data;

namespace ClsFusionViewer.Converters
{
    public class IntToFloatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return 0;

            int i = (int)value;
            return (float)i / 10;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
