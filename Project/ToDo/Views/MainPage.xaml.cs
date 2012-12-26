using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using Microsoft.Phone.Controls;
using ToDo.ViewModel;
using ToDo.Model;
using ToDo.Controls;
using ToDo.Utils;

namespace ToDo
{
    public partial class MainPage : PhoneApplicationPage
    {
        private UIElement shownButtons = null;

        public MainPage()
        {
            InitializeComponent();

            this.DataContext = App.ViewModel;
        }

        private void TextBlock_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            FrameworkElement tbx = sender as FrameworkElement;
            StackPanel parent = tbx.Parent as StackPanel;

            if (shownButtons != null)
            {

                if (shownButtons != parent.Children[1])
                {
                    shownButtons.Visibility = System.Windows.Visibility.Collapsed;
                }
                else
                {
                    AnimationUtils.Fold(shownButtons);
                }
            }
            
            if ((shownButtons != parent.Children[1]) && ((shownButtons = parent.Children[1]) != null))
            {
                //AnimationUtils.Expand(shownButtons);
                AnimationUtils.Split(todayExpanderView, shownButtons as FrameworkElement);
            }
            else
            {
                shownButtons = null;
            }
        }

        private void TodayTitle_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            //if (todayToDoItemsListBox.Visibility == System.Windows.Visibility.Visible)
            //{
            //    todayToDoItemsListBox.Visibility = System.Windows.Visibility.Collapsed;
            //}
            //else
            //{
            //    todayToDoItemsListBox.Visibility = System.Windows.Visibility.Visible;
            //}
        }

        private void TomorrowTitle_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            //if (tomorrowToDoItemsListBox.Visibility == System.Windows.Visibility.Visible)
            //{
            //    tomorrowToDoItemsListBox.Visibility = System.Windows.Visibility.Collapsed;
            //}
            //else
            //{
            //    tomorrowToDoItemsListBox.Visibility = System.Windows.Visibility.Visible;
            //}
        }

        private void LaterTitle_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
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

        private void ScrollViewer_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        private void NoteButton_Click(object sender, RoutedEventArgs e)
        {
            ToDoItem item = (sender as FrameworkElement).DataContext as ToDoItem;
            if (item != null)
            {
                PopupWindow.ShowWindow(new NoteControl(item));
            }
            
        }

        private void FlagButton_Click(object sender, RoutedEventArgs e)
        {
            StackPanel panel = (sender as FrameworkElement).Parent as StackPanel;
            if (panel != null)
            {
                TextBlock flagTextBlock = panel.Children[0] as TextBlock;
                if (flagTextBlock != null)
                {
                    flagTextBlock.Text = (flagTextBlock.Text == "0") ? "1" : "0";
                    App.ViewModel.SaveChangesToDB();
                }
                
            }
        }

        private void showNewNote_click(object sender, EventArgs e)
        {
            PopupWindow.ShowWindow(new NoteControl());
        }

        private void showNewNote(object sender, EventArgs e)
        {
            PopupWindow.ShowWindow(new NoteControl());
        }

        private void Remind_Click(object sender, EventArgs e)
        {
            ToDoItem item = (sender as FrameworkElement).DataContext as ToDoItem;
            if (item != null)
            {
                App.TodoParams = item;
                NavigationService.Navigate(new Uri("ReminderPage.xaml", UriKind.Relative));
            }
        }
    }
}