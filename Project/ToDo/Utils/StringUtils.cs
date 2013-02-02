using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ToDo.Utils
{
    class StringUtils
    {
        public static string GetSubString(string str, int length, string suffix)
        {
            string temp = str;
            int j = 0;
            int k = 0;
            for (int i = 0; i < temp.Length; i++)
            {
                if (Regex.IsMatch(temp.Substring(i, 1), @"[\u4e00-\u9fa5]+"))
                {
                    j += 2;
                }
                else
                {
                    j += 1;
                }
                if (j <= length)
                {
                    k += 1;
                }
                if (j >= length)
                {
                    return temp.Substring(0, k) + suffix;
                }
            }
            return temp;
        }
    }
}
