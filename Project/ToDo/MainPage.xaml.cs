using System;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Phone.Controls;

using ToDo.Model;
using ToDo.ViewModel;

namespace ToDo
{
    public partial class MainPage : PhoneApplicationPage
    {
        private UIElement shownButtons = null;
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

                TileModel.updateTile();
            }
            
        }

        private void TextBlock_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (shownButtons != null)
            {
                shownButtons.Visibility = System.Windows.Visibility.Collapsed;
            }
            FrameworkElement tbx = sender as FrameworkElement;
            StackPanel parent = tbx.Parent as StackPanel;
            if((shownButtons = parent.Children[1]) != null)
            {
                shownButtons.Visibility = System.Windows.Visibility.Visible;
            }
            
        }

        private void TodayTitle_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if(todayToDoItemsListBox.Visibility == System.Windows.Visibility.Visible)
            {
                todayToDoItemsListBox.Visibility = System.Windows.Visibility.Collapsed;
            }
            else
            {
                todayToDoItemsListBox.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void TomorrowTitle_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (tomorrowToDoItemsListBox.Visibility == System.Windows.Visibility.Visible)
            {
                tomorrowToDoItemsListBox.Visibility = System.Windows.Visibility.Collapsed;
            }
            else
            {
                tomorrowToDoItemsListBox.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void LaterTitle_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (laterToDoItemsListBox.Visibility == System.Windows.Visibility.Visible)
            {
                laterToDoItemsListBox.Visibility = System.Windows.Visibility.Collapsed;
            }
            else
            {
                laterToDoItemsListBox.Visibility = System.Windows.Visibility.Visible;
            }
        }
    }
}