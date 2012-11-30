using System;
using System.Windows;
using System.Windows.Controls;
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

        private void TextBlock_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox tbx = sender as TextBox;
            StackPanel parent = tbx.Parent as StackPanel;
            parent.Children[1].Visibility = System.Windows.Visibility.Visible;
        }

        private void TextBlock_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox tbx = sender as TextBox;
            StackPanel parent = tbx.Parent as StackPanel;
            parent.Children[1].Visibility = System.Windows.Visibility.Collapsed;
        }
    }
}