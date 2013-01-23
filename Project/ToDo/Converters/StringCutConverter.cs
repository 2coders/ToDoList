using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows;

namespace ToDo.Converters
{
    public class StringCutConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string str = value.ToString();
            int maxLength = 20;
            if (parameter != null)
            {
                maxLength = System.Convert.ToInt32(parameter);
            }
            if(str.Length > maxLength)
            {
                str = str.Substring(0, maxLength) + "...";
            }
            return str;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}