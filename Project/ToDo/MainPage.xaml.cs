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
using Microsoft.Phone.Controls;

using ToDo.Model;

namespace ToDo
{
    public partial class MainPage : PhoneApplicationPage
    {
        // 构造函数
        public MainPage()
        {
            InitializeComponent();

            this.DataContext = App.ViewModel;
        }

        private void deleteTaskButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void add_Click(object sender, RoutedEventArgs e)
        {
            if (newTodoItem.Text.Length > 0)
            {
                ToDoItem item = new ToDoItem();
                item.Title = newTodoItem.Text;
                item.CreateTime = DateTime.Now;
                item.RemindTime = DateTime.Now;
                item.IsCompleted = false;
                item.Note = "";
                item.Priority = 0;

                App.ViewModel.AddToDoItem(item);
                newTodoItem.Text = "";
            }
            
        }
    }
}