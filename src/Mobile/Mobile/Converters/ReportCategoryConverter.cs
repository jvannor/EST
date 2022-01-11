using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;
using Mobile.Models;

namespace Mobile.Converters
{
    public class ReportCategoryConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var report = (Report)value;
            var builder = new StringBuilder();

            string prefix = "";
            if ((!string.IsNullOrEmpty(report.Category) && (report.Category.Length > 1)))
            {
                builder.Append(report.Category);
                prefix = "; ";
            }
            else
            {
                prefix = "";
            }

            if ((!string.IsNullOrEmpty(report.Subcategory) && (report.Subcategory.Length > 1)))
            {
                builder.Append($"{prefix}{report.Subcategory}");
                prefix = "; ";
            }
            else
            {
                prefix = "";
            }
               
            if ((!string.IsNullOrEmpty(report.Detail) && (report.Detail.Length > 1)))
            {
                builder.Append($"{prefix}{report.Detail}");
            }

            return builder.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
