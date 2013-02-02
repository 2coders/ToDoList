using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows;
using ToDo.Utils;

namespace ToDo.Converters
{
    public class StringCutConverter : IValueConverter
    {
        static private bool isShowAll = false;

        static public void ShowAll(object o, string filed, string value)
        {
            isShowAll = true;
            Type t = o.GetType();
            t.GetProperty(filed).SetValue(o, value, null);
        }

        static public void ShowPart(object o, string filed, string value)
        {
            isShowAll = false;
            Type t = o.GetType();
            t.GetProperty(filed).SetValue(o, value, null);
        }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (isShowAll)
            {
                return value;
            }
            string str = value.ToString();
            int maxLength = 20;
            if (parameter != null)
            {
                maxLength = System.Convert.ToInt32(parameter);
            }
            return StringUtils.GetSubString(str, maxLength, "...");
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}