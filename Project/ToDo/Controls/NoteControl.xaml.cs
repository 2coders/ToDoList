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
using ToDo.Model;

namespace ToDo.Controls
{
    public partial class NoteControl : UserControl, IPopupedControl
    {
        public event PopupEventHandler Closed;
        public event PopupEventHandler Opened;

        public NoteControl()
        {
            InitializeComponent();
        }

        public NoteControl(ToDoItem item)
            : this()
        {
            this.DataContext = item;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.Opened != null)
            {
                this.Opened(this, new PopupEventArgs());
            }
            NoteTextBox.Focus();
            NoteTextBox.SelectionStart = NoteTextBox.Text.Length;
        }

        private void ContentTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            App.ViewModel.SaveChangesToDB();
            PopupWindow.HideWindow();
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            if (this.Closed != null)
            {
                this.Closed(this, new PopupEventArgs());
            }
        }
    }
}
