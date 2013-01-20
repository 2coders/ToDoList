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
    public partial class CreateItemControl : UserControl, IPopupedControl
    {
        public const String TODAY = "Today";
        public const String TOMORROW = "Tomorrow";
        public const String LATER = "Later";

        public event EventHandler Closed;
        public event EventHandler Opened;

        private string _GroupName = null;
        public string GroupName
        {
            get
            {
                return _GroupName;
            }
            set
            {
                _GroupName = value;
                groupNameTxt.Text = _GroupName;
            }
        }

        public CreateItemControl()
        {
            InitializeComponent();
        }

        private void LayoutRoot_Loaded(object sender, RoutedEventArgs e)
        {
            if (Opened != null)
            {
                Opened(this, new EventArgs());
            }
            ContentTextBox.Focus();
            ContentTextBox.SelectionStart = ContentTextBox.Text.Length;
        }

        private void ContentTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            String content;
            if ((content = ContentTextBox.Text.Trim()).Length > 0)
            {
                AddNewItem(content);
                App.ViewModel.SaveChangesToDB();
                Log.Info(this.ToString(), "new item created");
            }
            PopupWindow.HideWindow();
            if (this.Closed != null)
            {
                this.Closed(this, new EventArgs());
            }
        }

        #region 增加新项目
        private void AddNewItem(string content)
        {
            ToDoItem item = new ToDoItem();
            item.Title = content;
            item.CreateTime = DateTime.Now;
            item.IsCompleted = false;
            item.Note = "";
            item.Priority = 0;

            if (_GroupName != null && _GroupName.Equals(TOMORROW))
            {
                item.RemindTime = DateTime.Now.AddDays(1);
            }
            else if (_GroupName != null && _GroupName.Equals(LATER))
            {
                item.RemindTime = DateTime.Now.AddDays(2);
            }
            else
            {
                item.RemindTime = DateTime.Now;
            }
            
            App.ViewModel.AddToDoItem(item, _GroupName);
        }
        #endregion
    }
}
