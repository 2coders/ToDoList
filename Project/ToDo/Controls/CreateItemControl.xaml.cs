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
    public partial class CreateItemControl : UserControl
    {
        public CreateItemControl()
        {
            InitializeComponent();
        }

        private void LayoutRoot_Loaded(object sender, RoutedEventArgs e)
        {
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
        }

        #region 增加新项目
        private void AddNewItem(string content)
        {
            ToDoItem item = new ToDoItem();
            item.Title = content;
            item.CreateTime = DateTime.Now;
            item.RemindTime = DateTime.Now;
            item.IsCompleted = false;
            item.Note = "";
            item.Priority = 0;
            App.ViewModel.AddToDoItem(item);
        }
        #endregion
    }
}
