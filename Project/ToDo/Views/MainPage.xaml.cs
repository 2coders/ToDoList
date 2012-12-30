using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using Microsoft.Phone.Controls;
using ToDo.Controls;
using ToDo.Model;
using ToDo.Utils;

namespace ToDo
{
    public partial class MainPage : PhoneApplicationPage
    {
        private const string TAG = "MainPage";

        private StackPanel mCurrentItemPanel = null;

        public MainPage()
        {
            InitializeComponent();

            this.DataContext = App.ViewModel;
        }

        /// <summary>
        /// Event Hander Function
        /// </summary>
        #region Event Hander Function

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
                if (parent == null)
                {
                    Log.Error(TAG, "GestureListener_Flick, parent is null.");
                    return;
                }
                ToDoItem item = parent.DataContext as ToDoItem;
                if (item != null)
                {
                    if (e.HorizontalVelocity > 0)//flick to right
                    {
                        item.IsCompleted = true;
                        this.SetItemCompleted(parent);

                    }
                    else if (e.HorizontalVelocity < 0)//flick to left
                    {
                        this.SetItemNotCompleted(parent, item);
                    }
                }
            }
        }

        private void Border_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            FrameworkElement tbx = sender as FrameworkElement;
            if (tbx == null)
            {
                Log.Error(TAG, "Border_Tap, tbx is null.");
                return;
            }
            StackPanel parent = tbx.Parent as StackPanel;
            if (parent == null)
            {
                Log.Error(TAG, "Border_Tap, parent is null.");
                return;
            }
            
            if (mCurrentItemPanel != null)
            {
                HideItemDetails(mCurrentItemPanel);
            }

            if (mCurrentItemPanel != parent)
            {
                mCurrentItemPanel = parent;
                this.ShowItemDetails(mCurrentItemPanel);
            }
            else
            {
                mCurrentItemPanel = null;
            }
        }

        #endregion

        /// <summary>
        /// Private Function
        /// </summary>
        #region Private Function

        private void ShowItemDetails(StackPanel parent)
        {
            Storyboard storyboard = new Storyboard();

            FrameworkElement toolbar = parent.FindName("ToolBar") as FrameworkElement;
            AnimationUtils.SetHeightAnimation(storyboard, toolbar, 50, 0.2);

            FrameworkElement modifyButton = parent.FindName("ModifyButton") as FrameworkElement;
            AnimationUtils.SetOpacityAnimation(storyboard, modifyButton, 1, 0.3);

            storyboard.Begin();
        }

        private void HideItemDetails(StackPanel parent)
        {
            Storyboard storyboard = new Storyboard();

            FrameworkElement toolbar = parent.FindName("ToolBar") as FrameworkElement;
            AnimationUtils.SetHeightAnimation(storyboard, toolbar, AnimationUtils.AnimationHeightHide, 0.3);

            FrameworkElement modifyButton = parent.FindName("ModifyButton") as FrameworkElement;
            AnimationUtils.SetOpacityAnimation(storyboard, modifyButton, 0, 0.3);

            storyboard.Begin();
        }

        private void SetItemCompleted(StackPanel parent)
        {
            Storyboard storyboard = new Storyboard();

            FrameworkElement completedLine = parent.FindName("CompletedLine") as FrameworkElement;
            AnimationUtils.SetWidthAnimation(storyboard, completedLine, 400, 0.3);
            if (mCurrentItemPanel == parent)
            {
                FrameworkElement toolbar = parent.FindName("ToolBar") as FrameworkElement;
                AnimationUtils.SetHeightAnimation(storyboard, toolbar, AnimationUtils.AnimationHeightHide, 0.3);

                FrameworkElement modifyButton = parent.FindName("ModifyButton") as FrameworkElement;
                AnimationUtils.SetOpacityAnimation(storyboard, modifyButton, 0, 0.3);

                mCurrentItemPanel = null;
            }

            storyboard.Begin();
        }

        private void SetItemNotCompleted(StackPanel parent, ToDoItem item)
        {
            Storyboard storyboard = new Storyboard();

            FrameworkElement completedLine = parent.FindName("CompletedLine") as FrameworkElement;
            AnimationUtils.SetWidthAnimation(storyboard, completedLine, 0, 0.3);

            storyboard.Completed += delegate(object sender, EventArgs e)
            {
                item.IsCompleted = false;
            };

            storyboard.Begin();
        }

        #endregion
    }
}