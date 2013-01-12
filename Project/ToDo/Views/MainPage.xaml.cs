﻿using System;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Phone.Controls;
using ToDo.Controls;
using ToDo.Model;
using ToDo.Utils;
using Microsoft.Phone.Shell;

namespace ToDo
{
    public partial class MainPage : PhoneApplicationPage
    {
        private const String TAG = "MainPage";

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
            CreateItem(CreateItemControl.TODAY);
        }

        private void NoteButton_Click(object sender, RoutedEventArgs e)
        {
            ToDoItem item = (sender as FrameworkElement).DataContext as ToDoItem;
            if (item != null)
            {
                NoteControl note = new NoteControl(item);
                SetPopupedControlEvent(note);
                PopupWindow.ShowWindow(note);
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
            FrameworkElement deleteBtn = sender as FrameworkElement;
            DeleteItem(deleteBtn);
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
                        this.SetItemUnCompleted(parent, item);
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

        private void ModifyButton_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            ToDoItem item = (sender as FrameworkElement).DataContext as ToDoItem;
            if (item == null)
            {
                return;
            }

            var transform = (sender as Button).TransformToVisual(Application.Current.RootVisual);
            var pointOffset = transform.Transform(new Point(0, 0));

            double verticalOffset = pointOffset.Y - 600;

            ModifyTitleControl modify = new ModifyTitleControl(item) 
            {
                TitleMargin = new Thickness(pointOffset.X, (verticalOffset > 0) ? 600 : pointOffset.Y, 0, 0)
            };
            SetPopupedControlEvent(modify);

            if (verticalOffset > 0)
            {
                var storyboard = AnimationUtils.GetStoryboard();
                AnimationUtils.SetHeightAnimation(storyboard, VacancyStackPanel as FrameworkElement, verticalOffset, 0.3);
                AnimationUtils.SetAnyAnimation(storyboard, this as FrameworkElement, ScrowViewerVerticalOffsetProperty,
                    MainScrollViewer.VerticalOffset, MainScrollViewer.VerticalOffset + verticalOffset, 0.3);
                storyboard.Completed += delegate(object sender1, EventArgs e1)
                {
                    PopupWindow.ShowWindow(modify);
                };
                storyboard.Begin();
            }
            else 
            {
                PopupWindow.ShowWindow(modify);
            }
            
            e.Handled = true;
        }

        #endregion

        /// <summary>
        /// ScrollVeiwer ScrollTo Animation Extend
        /// </summary>
        #region ScrollVeiwer ScrollTo Animation Extension

        private DependencyProperty ScrowViewerVerticalOffsetProperty =
            DependencyProperty.Register("ScrollViewerVerticalOffset",
            typeof(double),
            typeof(MainPage),
            new PropertyMetadata(0.0, new PropertyChangedCallback(OnVerticalChanged))
            );

        private static void OnVerticalChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MainPage page = d as MainPage;
            double value = Convert.ToDouble(e.NewValue);
            page.MainScrollViewer.ScrollToVerticalOffset(value);
        }

        private double ScrollViewerVerticalOffset
        {
            get
            {
                return Convert.ToDouble(this.GetValue(ScrowViewerVerticalOffsetProperty));
            }
            set
            {
                this.SetValue(ScrowViewerVerticalOffsetProperty, value);
            }
        }

        #endregion

        /// <summary>
        /// Private Function
        /// </summary>
        #region Private Function

        private void SetPopupedControlEvent(IPopupedControl control)
        {
            if (control != null)
            {
                control.Opened += delegate(object sender2, EventArgs e2)
                {
                    ChangeApplicationBarButton(ApplicationBarConstant.Done);
                };
                control.Closed += delegate(object sender2, EventArgs e2)
                {
                    ChangeApplicationBarButton(ApplicationBarConstant.Add);
                };
            }
        }

        public enum ApplicationBarConstant {Add, Done};

        private void ChangeApplicationBarButton(ApplicationBarConstant flag)
        {
            var btn = this.ApplicationBar.Buttons[0] as ApplicationBarIconButton;
            if (flag == ApplicationBarConstant.Add)
            {
                btn.IconUri = new Uri("/Images/add.png", UriKind.Relative);
                btn.Text = "新建";
            }
            else if (flag == ApplicationBarConstant.Done)
            {
                btn.IconUri = new Uri("/Images/done.png", UriKind.Relative);
                btn.Text = "完成";
            }
        }

        private void CreateItem(String groupName)
        {
            var transform = todayExpanderView.TransformToVisual(Application.Current.RootVisual);
            var pointOffset = transform.Transform(new Point(0, 0));
            double verticalOffset = pointOffset.Y - 50;

            var createItem = new CreateItemControl()
            {
                GroupName = groupName
            };
            SetPopupedControlEvent(createItem);

            if (verticalOffset == 0)
            {
                PopupWindow.ShowWindow(createItem);
            }
            else
            {
                var storyboard = AnimationUtils.GetStoryboard();
                if (verticalOffset > 0)
                {
                    AnimationUtils.SetHeightAnimation(storyboard, VacancyStackPanel as FrameworkElement, verticalOffset + 1000, 0.3);
                }
                AnimationUtils.SetAnyAnimation(storyboard, this as FrameworkElement, ScrowViewerVerticalOffsetProperty,
                                               MainScrollViewer.VerticalOffset, MainScrollViewer.VerticalOffset + verticalOffset, 0.3);

                storyboard.Completed += delegate(object sender, EventArgs e)
                {
                    createItem.Closed += delegate(object sender2, EventArgs e2)
                    {
                        if (verticalOffset > 0)
                        {
                            var storyboard2 = AnimationUtils.GetStoryboard();
                            AnimationUtils.SetHeightAnimation(storyboard2, VacancyStackPanel as FrameworkElement, 0, 0.3);
                            storyboard2.Begin();
                        }
                    };
                    PopupWindow.ShowWindow(createItem);
                };
                storyboard.Begin();
            }
        }

        private void DeleteItem(FrameworkElement deleteBtn)
        {
            var storyboard = AnimationUtils.GetStoryboard();
            FrameworkElement item = deleteBtn.FindName("ListItem") as FrameworkElement;
            AnimationUtils.SetOpacityAnimation(storyboard, item, 0, 0.3);
            AnimationUtils.SetHeightAnimation(storyboard, item, 0, 0.3);

            storyboard.Completed += delegate(object sender, EventArgs e)
            {
                App.ViewModel.DeleteToDoItem(item.DataContext as ToDoItem);
            };
            storyboard.Begin();
        }

        private void ShowItemDetails(StackPanel parent)
        {
            var storyboard = AnimationUtils.GetStoryboard();

            FrameworkElement toolbar = parent.FindName("ToolBar") as FrameworkElement;
            AnimationUtils.SetHeightAnimation(storyboard, toolbar, 50, 0.2);

            FrameworkElement modifyButton = parent.FindName("ModifyButton") as FrameworkElement;
            AnimationUtils.SetOpacityAnimation(storyboard, modifyButton, 1, 0.3);

            storyboard.Begin();
        }

        private void HideItemDetails(StackPanel parent)
        {
            var storyboard = AnimationUtils.GetStoryboard();

            FrameworkElement toolbar = parent.FindName("ToolBar") as FrameworkElement;
            AnimationUtils.SetHeightAnimation(storyboard, toolbar, AnimationUtils.AnimationHeightHide, 0.3);

            FrameworkElement modifyButton = parent.FindName("ModifyButton") as FrameworkElement;
            AnimationUtils.SetOpacityAnimation(storyboard, modifyButton, 0, 0.3);

            storyboard.Begin();
        }

        private void SetItemCompleted(StackPanel parent)
        {
            var storyboard = AnimationUtils.GetStoryboard();

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

        private void SetItemUnCompleted(StackPanel parent, ToDoItem item)
        {
            var storyboard = AnimationUtils.GetStoryboard();

            FrameworkElement completedLine = parent.FindName("CompletedLine") as FrameworkElement;
            AnimationUtils.SetWidthAnimation(storyboard, completedLine, 0, 0.3);

            storyboard.Completed += delegate(object sender, EventArgs e)
            {
                item.IsCompleted = false;
            };

            storyboard.Begin();
        }

        #endregion

        private void AddItem_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            //CreateItem();
            e.Handled = true;
        }

    }
}