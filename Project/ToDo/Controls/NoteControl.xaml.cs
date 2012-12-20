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



        private void button2_Click(object sender, RoutedEventArgs e)
        {
            PopupWindow.HideWindow();
        }

        private void LayoutRoot_Loaded(object sender, RoutedEventArgs e)
        {
            ContentTextBox.Focus();
        }

        private void positiveBtn_Click(object sender, RoutedEventArgs e)
        {
            string content = ContentTextBox.Text;
            if (content.Length > 0)
            {
                addNewsItem(content);
                ContentTextBox.Text = "";
                TileModel.updateTile();
                PopupWindow.HideWindow();
            }
        }

        #region 增加新项目
        private void addNewsItem(string content)
        {
            ToDoItem item = new ToDoItem();
            item.Title = content;
            item.CreateTime = DateTime.Now;
            item.RemindTime = DateTime.Now;
            item.IsCompleted = false;
            item.Note = "";
            item.Priority = 0;
            App.ViewModel.AddToDoItem(item);

            System.Diagnostics.Debug.WriteLine("Debug Message");
        }
        #endregion



    }
}
