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
using ToDo.ViewModel;

namespace ToDo.Controls
{
    public partial class NoteControl : UserControl
    {
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
            ContentTextBox.Focus();
            ContentTextBox.SelectionStart = ContentTextBox.Text.Length;
        }

        private void ContentTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            App.ViewModel.SaveChangesToDB();
            PopupWindow.HideWindow();
        }

        #region 增加新项目
        private void addNewsItem(string content)
        {
            ToDoItem item = new ToDoItem();
            item.Title = content;
            item.CreateTime = DateTime.Now;
            item.IsCompleted = false;
            item.Note = "";
            item.Priority = 0;
            App.ViewModel.AddToDoItem(item);

            System.Diagnostics.Debug.WriteLine("Debug Message");
        }
        #endregion

       


    }
}
