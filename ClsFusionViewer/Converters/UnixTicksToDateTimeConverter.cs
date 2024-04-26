using System;
using System.Globalization;
using System.Windows.Data;

namespace ClsFusionViewer.Converters
{
    public class UnixTicksToDateTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var utcTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var foo = value.GetType();

            if (value.GetType() == typeof(int))
                return utcTime.AddSeconds((int)value);

            else if (value.GetType() == typeof(Int64))
                return utcTime.AddSeconds((Int64)value);

            return utcTime.AddSeconds(long.Parse((string)value));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
