using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace Mobile.Converters
{
    public class ListToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var list = value as IEnumerable<string>;
            if (list == null)
                return null;

            var builder = new StringBuilder();
            var prefix = string.Empty;
            foreach(var element in list)
            {
                builder.Append(prefix);
                builder.Append(element);
                prefix = "; ";
            }
            return builder.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
