using System;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Phone.Controls;
using ToDo.Controls;
using ToDo.Model;
using ToDo.Utils;
using Microsoft.Phone.Shell;
using ToDo.Converters;

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

        private void CreateItem_Click(object sender, EventArgs e)
        {
            CreateItem(todayExpanderView, CreateItemControl.TODAY);
        }

        private void CleanItem_Click(object sender, EventArgs e)
        {
            MessageBoxResult result = MessageBox.Show(AppResources.CompletedItemsClearMessage, AppResources.Tip, MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                CleanAllCompletedItems();

                if (App.ViewModel.CompletedToDoItems == null || App.ViewModel.CompletedToDoItems.Count == 0)
                {
                    ApplicationBarIconButton btn = sender as ApplicationBarIconButton;
                    btn.IsEnabled = false;
                }
            }
        }

        private void DoneItem_Click(object sender, EventArgs e)
        {
            PopupWindow.HideWindow();
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

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            FrameworkElement deleteBtn = sender as FrameworkElement;
            DeleteItem(deleteBtn);
        }

        private void GestureListener_Flick(object sender, FlickGestureEventArgs e)
        {
            StackPanel p = sender as StackPanel;
            Log.Info("xxx", e.VerticalVelocity.ToString());
            Log.Info("xxx", e.HorizontalVelocity.ToString());
            Log.Info("xxx", e.Angle.ToString());
            Log.Info("xxx", e.Direction.ToString());
            Log.Info("xxx", e.GetPosition(p).X.ToString());
            Log.Info("xxx", e.GetPosition(p).X.ToString());

            if (PopupWindow.IsShown)
            {
                return;
            }

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
                    if (e.HorizontalVelocity > 1000 && Math.Abs(e.VerticalVelocity) < 300)//flick to right
                    {
                        this.SetItemCompleted(parent, item);
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

            ToDoItem item = tbx.DataContext as ToDoItem;
            if (item == null)
            {
                Log.Error(TAG, "Border_Tap, item is null.");
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
            var parent = (sender as FrameworkElement).Parent as StackPanel;
            ModifyItemTitle(parent, item);
            e.Handled = true;
        }

        private void TodayNewButton_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            CreateItem(todayExpanderView, CreateItemControl.TODAY);
            e.Handled = true;
        }

        private void TomorrowNewButton_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            CreateItem(tomorrowExpanderView, CreateItemControl.TOMORROW);
            e.Handled = true;
        }

        private void LaterNewButton_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            CreateItem(laterExpanderView, CreateItemControl.LATER);
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
                control.Opened += delegate(object sender, PopupEventArgs e)
                {
                    ChangeApplicationBarButton(ApplicationBarConstant.Done);
                    SetMenuItemVisibility(false);
                    
                };
                control.Closed += delegate(object sender, PopupEventArgs e)
                {
                    ChangeApplicationBarButton(ApplicationBarConstant.Add);
                    SetMenuItemVisibility(true);
                };
            }
        }


        private void CreateItem(ExpanderView list, String groupName)
        {
            var transform = list.TransformToVisual(Application.Current.RootVisual);
            var pointOffset = transform.Transform(new Point(0, 0));
            double verticalOffset = pointOffset.Y - 50;

            var createItem = new CreateItemControl()
            {
                GroupName = groupName
            };
            SetPopupedControlEvent(createItem);

            createItem.Closed += delegate(object sender, PopupEventArgs e)
            {
                list.IsExpanded = true;
                if (verticalOffset > 0)
                {
                    var storyboard2 = AnimationUtils.GetStoryboard();
                    AnimationUtils.SetHeightAnimation(storyboard2, VacancyStackPanel as FrameworkElement, 0, 0.3);
                    storyboard2.Begin();
                }

                if (e.Done)//Expand the new item
                {
                    FrameworkElement item = list.Items[0] as FrameworkElement;
                    if (item != null)
                    {
                        StackPanel panel = item.FindName("ItemPanel") as StackPanel;
                        if (panel != null)
                        {
                            if (mCurrentItemPanel != null)
                            {
                                HideItemDetails(mCurrentItemPanel);
                            }
                            ShowItemDetails(panel);
                            mCurrentItemPanel = panel;
                        }
                    }
                }

            };

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
                    PopupWindow.ShowWindow(createItem);
                };
                storyboard.Begin();
            }
        }

        private void CleanAllCompletedItems()
        {
            App.ViewModel.DeleteAllCompletedItems();
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
            var title = parent.FindName("ItemTitleText") as TextBlock;
            title.TextWrapping = TextWrapping.Wrap;

            var storyboard = AnimationUtils.GetStoryboard();

            FrameworkElement toolbar = parent.FindName("ToolBar") as FrameworkElement;
            AnimationUtils.SetHeightAnimation(storyboard, toolbar, 80, 0.3);
            AnimationUtils.SetOpacityAnimation(storyboard, toolbar, 1, 0.3);

            FrameworkElement modifyButton = parent.FindName("ModifyButton") as FrameworkElement;
            modifyButton.Visibility = Visibility.Visible;
            AnimationUtils.SetOpacityAnimation(storyboard, modifyButton, 1, 0.3);

            storyboard.Begin();
        }

        private void HideItemDetails(StackPanel parent)
        {
            var title = parent.FindName("ItemTitleText") as TextBlock;
            title.TextWrapping = TextWrapping.NoWrap;

            var storyboard = AnimationUtils.GetStoryboard();

            FrameworkElement toolbar = parent.FindName("ToolBar") as FrameworkElement;
            AnimationUtils.SetHeightAnimation(storyboard, toolbar, AnimationUtils.AnimationHeightHide, 0.2);
            AnimationUtils.SetOpacityAnimation(storyboard, toolbar, 0, 0.2);

            FrameworkElement modifyButton = parent.FindName("ModifyButton") as FrameworkElement;
            AnimationUtils.SetOpacityAnimation(storyboard, modifyButton, 0, 0.2);

            storyboard.Completed += delegate(object sender, EventArgs e)
            {
                modifyButton.Visibility = Visibility.Collapsed;
            };

            storyboard.Begin();
        }

        private void SetItemCompleted(StackPanel parent, ToDoItem item)
        {
            if (item == null || parent == null)
            {
                return;
            }

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

            var storyboard2 = AnimationUtils.GetStoryboard();
            FrameworkElement listItem = parent.FindName("ListItem") as FrameworkElement;
            AnimationUtils.SetTranslateAnimation(storyboard2, parent, 0, Application.Current.Host.Content.ActualHeight, 0.5);
            AnimationUtils.SetHeightAnimation(storyboard2, listItem, 0, 0.5);

            item.IsCompleted = true;
            storyboard.Completed += delegate(object sender, EventArgs e)
            {
                storyboard2.Begin();
            };

            storyboard2.Completed += delegate(object sender, EventArgs e)
            {
                App.ViewModel.ChangeCompletedStatus(item, true);
            };

            storyboard.Begin();
        }

        private void SetItemUnCompleted(StackPanel parent, ToDoItem item)
        {
            if (item == null || parent == null)
            {
                return;
            }

            var storyboard = AnimationUtils.GetStoryboard();

            FrameworkElement completedLine = parent.FindName("CompletedLine") as FrameworkElement;
            AnimationUtils.SetWidthAnimation(storyboard, completedLine, 0, 0.3);

            storyboard.Completed += delegate(object sender, EventArgs e)
            {
                item.IsCompleted = false;
            };

            storyboard.Begin();
        }

        private void ModifyItemTitle(StackPanel parent, ToDoItem item)
        {
            if (item == null || parent == null)
            {
                return;
            }

            const int topOffset = 180;

            var text = parent.FindName("ItemTitleText") as FrameworkElement;
            var transform = text.TransformToVisual(Application.Current.RootVisual);
            var pointOffset = transform.Transform(new Point(0, 0));
            Log.Info(TAG, "pointOffset.Y:" + pointOffset.Y.ToString());
            double verticalOffset = pointOffset.Y - topOffset;

            ModifyTitleControl modify = new ModifyTitleControl(item)
            {
                TitleHeight = (verticalOffset > 0) ? topOffset : pointOffset.Y
            };
            SetPopupedControlEvent(modify);

            modify.Closed += delegate(object sender, PopupEventArgs e)
            {
                if (verticalOffset > 0)
                {
                    var storyboard2 = AnimationUtils.GetStoryboard();
                    AnimationUtils.SetHeightAnimation(storyboard2, VacancyStackPanel as FrameworkElement, 0, 0.3);
                    storyboard2.Begin();
                }
            };

            if (verticalOffset > 0)
            {
                var storyboard = AnimationUtils.GetStoryboard();
                AnimationUtils.SetHeightAnimation(storyboard, VacancyStackPanel as FrameworkElement, verticalOffset + 1000, 0.3);
                AnimationUtils.SetAnyAnimation(storyboard, this as FrameworkElement, ScrowViewerVerticalOffsetProperty,
                    MainScrollViewer.VerticalOffset, MainScrollViewer.VerticalOffset + verticalOffset, 0.3);
                storyboard.Completed += delegate(object sender1, EventArgs e1)
                {
                    PopupWindow.ShowWindow(modify, PopupWindow.PopupWindowBackgroundType.Flash);
                };
                storyboard.Begin();
            }
            else
            {
                PopupWindow.ShowWindow(modify, PopupWindow.PopupWindowBackgroundType.Flash);
            }
            
        }

        #endregion


        #region ApplicationBar

        private ApplicationBarConstant currentAppBarFlag;
        public enum ApplicationBarConstant { Add, Done, Clean };

        private void ChangeApplicationBarButton(ApplicationBarConstant flag)
        {
            this.ApplicationBar.Buttons.Clear();
            ApplicationBarIconButton btn = new ApplicationBarIconButton();
            btn.IsEnabled = true;
            if (flag == ApplicationBarConstant.Add)
            {
                btn.IconUri = new Uri("/Images/add.png", UriKind.Relative);
                btn.Text = AppResources.New;
                btn.Click += CreateItem_Click;
                this.ApplicationBar.Buttons.Add(btn);
                RemoveSecondButton();
            }
            else if (flag == ApplicationBarConstant.Done)
            {
                btn.IconUri = new Uri("/Images/done.png", UriKind.Relative);
                btn.Text = AppResources.Done;
                btn.Click += DoneItem_Click;

                ApplicationBarIconButton cancel = new ApplicationBarIconButton();
                cancel.IconUri = new Uri("/Images/cancel.png", UriKind.Relative);
                cancel.Text = AppResources.Cancel;
                this.ApplicationBar.Buttons.Add(btn);
                this.ApplicationBar.Buttons.Add(cancel);

                cancel.Click += delegate(object sender, EventArgs e)
                {
                    PopupWindow.HideWindow(true);
                };
            }
            else if (flag == ApplicationBarConstant.Clean)  // Completed items delete
            {
                btn.IconUri = new Uri("/Images/delete.png", UriKind.Relative);
                btn.Text = AppResources.Clean;
                btn.Click += CleanItem_Click;
                this.ApplicationBar.Buttons.Add(btn);
                RemoveSecondButton();

                if (App.ViewModel.CompletedToDoItems == null || App.ViewModel.CompletedToDoItems.Count == 0)
                {
                    btn.IsEnabled = false;
                }
            }
            
            currentAppBarFlag = flag;
        }

        private void SetMenuItemVisibility(Boolean show)
        {
            if (show)
            {
                this.ApplicationBar.MenuItems.Clear();
                ApplicationBarMenuItem completedItem = new ApplicationBarMenuItem();
                completedItem.Text = AppResources.CompletedItemsHeader;
                completedItem.Click += CompletedMenuItem_Click;
                ApplicationBarMenuItem settingItem = new ApplicationBarMenuItem();
                settingItem.Text = AppResources.Setting;
                settingItem.Click += SettingMenuItem_Click;

                this.ApplicationBar.MenuItems.Add(completedItem);
                this.ApplicationBar.MenuItems.Add(settingItem);

            }
            else
            {
                this.ApplicationBar.MenuItems.Clear();
            }
        }

        private void RemoveSecondButton()
        {
            if (this.ApplicationBar.Buttons.Count > 1)
            {
                this.ApplicationBar.Buttons.RemoveAt(1);
            }
        }

        private CompletedItemListControl completedControl;
        private void CompletedMenuItem_Click(object sender, EventArgs e)
        {
            if (completedControl == null)
            {
                completedControl = new CompletedItemListControl(HeightUtils.GoldSectionHeight(this)) { MainScrollViewer = this.MainScrollViewer };
                PopupWindow.ShowWindow(completedControl, PopupWindow.PopupWindowBackgroundType.None);
                completedControl.Opened += delegate(object sender1, PopupEventArgs e1)
                {
                    SetMenuItemVisibility(false);

                };
                completedControl.Closed += delegate(object sender1, PopupEventArgs e1)
                {
                    SetMenuItemVisibility(true);
                    ChangeApplicationBarButton(ApplicationBarConstant.Add);
                    completedControl = null;
                };
                ChangeApplicationBarButton(ApplicationBarConstant.Clean);
            }
        }

        private void SettingMenuItem_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/SettingPage.xaml", UriKind.Relative));
        }

        #endregion

        private void PhoneApplicationPage_BackKeyPress(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (currentAppBarFlag == ApplicationBarConstant.Clean)
            {
                if (completedControl != null)
                {
                    completedControl.HideWindow();
                    e.Cancel = true;
                }
            }
        }

        private void OnPageLoaded(object sender, RoutedEventArgs e)
        {
            ChangeApplicationBarButton(ApplicationBarConstant.Add);
            SetMenuItemVisibility(true);
        }

        private void PhoneApplicationPage_Unloaded(object sender, RoutedEventArgs e)
        {
            Log.Info(TAG, "PhoneApplicationPage_Unloaded");
        }


    }
}