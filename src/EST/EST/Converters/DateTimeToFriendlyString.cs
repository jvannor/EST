using System;
using System.Globalization;
using Xamarin.Forms;

namespace EST.Converters
{
    public class DateTimeToFriendlyString : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var source = (DateTime)value;
            var diff = (DateTime.Now.Date - source.Date).Days;

            var result = string.Format("{0:d}", source);
            if (diff == 0)
                result = "Today";
            else if (diff == 1)
                result = "Yesterday";
            else if ((diff > 1) && (diff < 7))
                result = string.Format("{0:dddd}", source);

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
