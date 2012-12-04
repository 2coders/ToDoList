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

namespace ToDo.Controls
{
    public partial class NoteControl : UserControl
    {
        public NoteControl()
        {
            InitializeComponent();
        }



        private void button2_Click(object sender, RoutedEventArgs e)
        {
            PopupWindow.HideWindow();
        }

        private void LayoutRoot_Loaded(object sender, RoutedEventArgs e)
        {
            ContentTextBox.Focus();
        }
    }
}
