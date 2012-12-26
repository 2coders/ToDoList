using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Diagnostics;

namespace ToDo.Utils
{
    public class Log
    {
        static public void Info(String tag, String message)
        {
            Debug.WriteLine("{0}    :{1}", tag, message);
        }

        static public void Error(String tag, String message)
        {
            Debug.WriteLine("{0}    :{1}", tag, message);
        }
    }
}
