using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using MessageControl;

namespace Test
{
    public partial class WindowsPhoneControl1 : UserControl
    {
        public WindowsPhoneControl1()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            MyMessage.HideWindow();
        }
    }
}
