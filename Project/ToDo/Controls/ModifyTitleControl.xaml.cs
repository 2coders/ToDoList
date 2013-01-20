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
using ToDo.Utils;

namespace ToDo.Controls
{
    public partial class ModifyTitleControl : UserControl, IPopupedControl
    {

        public event EventHandler Closed;
        public event EventHandler Opened;

        private String preTitle;

        public Thickness TitleMargin
        {
            get
            {
                return ModifyTextBox.Margin;
            }
            set
            {
                ModifyTextBox.Margin = value;
            }
        }

        public double TitleHeight
        {
            set
            {
                ModifyTextBox.Margin = new Thickness(ModifyTextBox.Margin.Left, value - 10, 0, 0);
            }
        }

        public ModifyTitleControl()
        {
            InitializeComponent();
        }

        public ModifyTitleControl(ToDoItem item)
            : this()
        {
            this.DataContext = item;
            this.preTitle = item.Title;
        }

        private void LayoutRoot_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.Opened != null)
            {
                this.Opened(this, new EventArgs());
            }
            ModifyTextBox.Focus();
            ModifyTextBox.SelectionStart = ModifyTextBox.Text.Length;
        }

        private void ContentTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (ModifyTextBox.Text.Trim() == "")
            {
                ModifyTextBox.Text = this.preTitle;
            }

            App.ViewModel.SaveChangesToDB();
            PopupWindow.HideWindow();
            if (this.Closed != null)
            {
                this.Closed(this, new EventArgs());
            }
        }

    }
}
