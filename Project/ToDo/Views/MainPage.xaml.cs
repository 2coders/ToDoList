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

        private void ScrollViewer_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        private void CreateItem_Click(object sender, EventArgs e)
        {
            PopupWindow.ShowWindow(new CreateItemControl());
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
            ToDoItem item = (sender as FrameworkElement).DataContext as ToDoItem;
            if (item != null)
            {
                //switch the priority between 0 and 1
                item.Priority = 1 - item.Priority;
                App.ViewModel.SaveChangesToDB();
            }
        }

        private void Remind_Click(object sender, EventArgs e)
        {
            ToDoItem item = (sender as FrameworkElement).DataContext as ToDoItem;
            if (item != null)
            {
                App.TodoParams = item;
                NavigationService.Navigate(new Uri("/Views/ReminderPage.xaml", UriKind.Relative));
            }
        }

        private void Complete_Click(object sender, RoutedEventArgs e)
        {
            FrameworkElement button = sender as FrameworkElement;
            ToDoItem item = button.DataContext as ToDoItem;
            if (item != null)
            {
                if (item.IsCompleted)
                {
                    item.IsCompleted = false;
                }
                else
                {
                    if (shownButtons != null)
                    {
                        shownButtons.Visibility = System.Windows.Visibility.Collapsed;
                    }
                    item.IsCompleted = true;
                    //StackPanel sp1 = button.Parent as StackPanel;
                    //StackPanel sp2 = sp1.Parent as StackPanel;
                    //StackPanel sp3 = sp2.Children[2] as StackPanel;
                    //AnimationUtils.LineTranslate(sp3.Children[0]);
                }
            }

        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            ToDoItem item = (sender as FrameworkElement).DataContext as ToDoItem;
            App.ViewModel.DeleteToDoItem(item);
        }

        private void GestureListener_Flick(object sender, FlickGestureEventArgs e)
        {
            if (e.Direction == System.Windows.Controls.Orientation.Horizontal)
            {
                StackPanel parent = sender as StackPanel;
                if(parent == null)
                {
                    return;
                }
                ToDoItem item = parent.DataContext as ToDoItem;
                if (item != null)
                {
                    if (e.HorizontalVelocity > 0)//flick to right
                    {
                        item.IsCompleted = true;
                        if (shownButtons != null && parent.FindName("ToolBar") == shownButtons)
                        {
                            AnimationUtils.ChangeHeight(shownButtons as FrameworkElement, AnimationUtils.AnimationHeightHide, 0.3);
                        }
                    }
                    else if (e.HorizontalVelocity < 0)//flick to left
                    {
                        item.IsCompleted = false;
                    }
                }
            }
        }

        private void Border_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            FrameworkElement tbx = sender as FrameworkElement;
            StackPanel parent = tbx.Parent as StackPanel;

            if (shownButtons != null)
            {

                if (shownButtons != parent.Children[1])
                {
                    AnimationUtils.ChangeHeight(shownButtons as FrameworkElement, AnimationUtils.AnimationHeightHide, 0);
                }
                else
                {
                    AnimationUtils.ChangeHeight(shownButtons as FrameworkElement, AnimationUtils.AnimationHeightHide, 0.3);
                }
            }

            if ((shownButtons != parent.Children[1]) && ((shownButtons = parent.Children[1]) != null))
            {
                AnimationUtils.ChangeHeight(shownButtons as FrameworkElement, 50, 0.2);
            }
            else
            {
                shownButtons = null;
            }
        }

    }
}