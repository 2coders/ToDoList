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

        private bool _completedPanelExpanded = false;
        public bool CompletedPanelExpanded 
        {
            get
            {
                return _completedPanelExpanded;
            }
            set
            {
                _completedPanelExpanded = value;
                //MainScrollViewer.VerticalScrollBarVisibility = _completedPanelExpanded ? ScrollBarVisibility.Disabled : ScrollBarVisibility.Auto;
            }
        }

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
            if (currentAppBarFlag == ApplicationBarConstant.Add)
            {
                CreateItem(todayExpanderView, CreateItemControl.TODAY);
            }
            else if (currentAppBarFlag == ApplicationBarConstant.Clean)
            {
                CleanAllCompletedItems();
            }
            
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
                    if (e.HorizontalVelocity > 0)//flick to right
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
                HideItemDetails(mCurrentItemPanel, item);
            }

            if (mCurrentItemPanel != parent)
            {
                mCurrentItemPanel = parent;
                this.ShowItemDetails(mCurrentItemPanel, item);
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
                control.Opened += delegate(object sender, EventArgs e)
                {
                    ChangeApplicationBarButton(ApplicationBarConstant.Done);
                    
                };
                control.Closed += delegate(object sender, EventArgs e)
                {
                    ChangeApplicationBarButton(ApplicationBarConstant.Add);
                };
            }
        }


        private void CreateItem(FrameworkElement list, String groupName)
        {
            var transform = list.TransformToVisual(Application.Current.RootVisual);
            var pointOffset = transform.Transform(new Point(0, 0));
            double verticalOffset = pointOffset.Y - 50;

            var createItem = new CreateItemControl()
            {
                GroupName = groupName
            };
            SetPopupedControlEvent(createItem);

            createItem.Opened += delegate(object sender, EventArgs e)
            {
                // 新建数据展开ExpanderView
                ((ExpanderView)list).IsExpanded = true;
            };

            createItem.Closed += delegate(object sender, EventArgs e)
            {
                if (verticalOffset > 0)
                {
                    var storyboard2 = AnimationUtils.GetStoryboard();
                    AnimationUtils.SetHeightAnimation(storyboard2, VacancyStackPanel as FrameworkElement, 0, 0.3);
                    storyboard2.Begin();
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

        private void ShowItemDetails(StackPanel parent, ToDoItem item)
        {
            StringCutConverter.ShowAll(item, "Title", item.Title);

            var storyboard = AnimationUtils.GetStoryboard();

            FrameworkElement toolbar = parent.FindName("ToolBar") as FrameworkElement;
            AnimationUtils.SetHeightAnimation(storyboard, toolbar, 50, 0.3);
            AnimationUtils.SetOpacityAnimation(storyboard, toolbar, 1, 0.3);

            FrameworkElement modifyButton = parent.FindName("ModifyButton") as FrameworkElement;
            AnimationUtils.SetOpacityAnimation(storyboard, modifyButton, 1, 0.3);

            storyboard.Begin();
        }

        private void HideItemDetails(StackPanel parent, ToDoItem item)
        {
            StringCutConverter.ShowPart(item, "Title", item.Title);

            var storyboard = AnimationUtils.GetStoryboard();

            FrameworkElement toolbar = parent.FindName("ToolBar") as FrameworkElement;
            AnimationUtils.SetHeightAnimation(storyboard, toolbar, AnimationUtils.AnimationHeightHide, 0.2);
            AnimationUtils.SetOpacityAnimation(storyboard, toolbar, 0, 0.2);

            FrameworkElement modifyButton = parent.FindName("ModifyButton") as FrameworkElement;
            AnimationUtils.SetOpacityAnimation(storyboard, modifyButton, 0, 0.2);

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

            storyboard.Completed += delegate(object sender, EventArgs e)
            {
                item.IsCompleted = true;
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

            int topOffset = 180;

            var text = parent.FindName("ItemTitleText") as FrameworkElement;
            var transform = text.TransformToVisual(Application.Current.RootVisual);
            var pointOffset = transform.Transform(new Point(0, 0));

            double verticalOffset = pointOffset.Y - topOffset;

            ModifyTitleControl modify = new ModifyTitleControl(item)
            {
                TitleHeight = (verticalOffset > 0) ? topOffset : pointOffset.Y
            };
            SetPopupedControlEvent(modify);

            modify.Closed += delegate(object sender, EventArgs e)
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
                AnimationUtils.SetHeightAnimation(storyboard, VacancyStackPanel as FrameworkElement, verticalOffset, 0.3);
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
            var btn = this.ApplicationBar.Buttons[0] as ApplicationBarIconButton;
            if (flag == ApplicationBarConstant.Add)
            {
                btn.IconUri = new Uri("/Images/add.png", UriKind.Relative);
                btn.Text = "新建";
                currentAppBarFlag = ApplicationBarConstant.Add;

                RemoveSecondButton();
            }
            else if (flag == ApplicationBarConstant.Done)
            {
                btn.IconUri = new Uri("/Images/done.png", UriKind.Relative);
                btn.Text = "完成";
                currentAppBarFlag = ApplicationBarConstant.Done;

                ApplicationBarIconButton cancel = new ApplicationBarIconButton();
                cancel.IconUri = new Uri("/Images/cancel.png", UriKind.Relative);
                cancel.Text = "取消";
                this.ApplicationBar.Buttons.Add(cancel);

                cancel.Click += delegate(object sender, EventArgs e)
                {
                    PopupWindow.HideWindow();
                };
            }
            else if (flag == ApplicationBarConstant.Clean)
            {
                btn.IconUri = new Uri("/Images/delete.png", UriKind.Relative);
                btn.Text = "清除";
                currentAppBarFlag = ApplicationBarConstant.Clean;

                RemoveSecondButton();
            }
        }

        private void RemoveSecondButton()
        {
            if (this.ApplicationBar.Buttons.Count > 1)
            {
                this.ApplicationBar.Buttons.RemoveAt(1);
            }
        }

        
        private void CompletedMenuItem_Click(object sender, EventArgs e)
        {
            CompletedItemListControl control = new CompletedItemListControl(HeightUtils.GoldSectionHeight(this)) { MainScrollViewer = this.MainScrollViewer };
            PopupWindow.ShowWindow(control, PopupWindow.PopupWindowBackgroundType.None);
            control.Closed += delegate(object sender1, EventArgs e1)
            {
                ChangeApplicationBarButton(ApplicationBarConstant.Add);
            };
            ChangeApplicationBarButton(ApplicationBarConstant.Clean);
        }

        #endregion

    }
}