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
        public event EventHandler Closed;
        public event EventHandler Opened;

        private double _YPosition = 0;
        public double YPosition
        {
            get 
            {
                return _YPosition;
            }
            set 
            {
                _YPosition = value;
                NoteTextBox.Margin = new Thickness(0, _YPosition, 0, 0);
            }
        }

        public NoteControl()
        {
            InitializeComponent();
        }

        public NoteControl(ToDoItem item)
            : this()
        {
            this.DataContext = item;
        }

        private void LayoutRoot_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.Opened != null)
            {
                this.Opened(this, new EventArgs());
            }
            NoteTextBox.Focus();
            NoteTextBox.SelectionStart = NoteTextBox.Text.Length;
        }

        private void ContentTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            App.ViewModel.SaveChangesToDB();
            PopupWindow.HideWindow();
            if (this.Closed != null)
            {
                this.Closed(this, new EventArgs());
            }
        }
    }
}
