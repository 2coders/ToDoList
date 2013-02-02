using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using ToDo.Utils;
using System.Windows.Media.Animation;

namespace ToDo.Controls
{
    public partial class CompletedItemListControl : UserControl, IPopupedControl
    {
        public event PopupEventHandler Closed;
        public event PopupEventHandler Opened;

        private double _height;

        private ScrollViewer _mainScrollViewer;
        public ScrollViewer MainScrollViewer
        {
            get
            {
                return _mainScrollViewer;
            }
            set
            {
                _mainScrollViewer = value;
            }
        }
          
        public CompletedItemListControl(double height)
        {
            InitializeComponent();
            _height = height;

            this.DataContext = App.ViewModel;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (Opened != null)
            {
                Opened(this, new PopupEventArgs());
            }

            CompletedStackPanel.Height = _height;
            var storyboard = AnimationUtils.GetStoryboard();
            AnimationUtils.SetTranslateAnimation(storyboard, _mainScrollViewer as FrameworkElement, 0, -_height, 0.6);
            AnimationUtils.SetTranslateAnimation(storyboard, CompletedStackPanel as FrameworkElement, _height, 0, 0.6);
            storyboard.Begin();

        }

        private void CompletedPanelTop_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            double completedPanelHeight = CompletedStackPanel.ActualHeight;
            var storyboard = AnimationUtils.GetStoryboard();
            AnimationUtils.SetTranslateAnimation(storyboard, _mainScrollViewer as FrameworkElement, -_height, 0, 0.6);
            AnimationUtils.SetTranslateAnimation(storyboard, CompletedStackPanel as FrameworkElement, 0, _height, 0.6);
            storyboard.Completed += delegate(object sender1, EventArgs e1)
            {
                PopupWindow.HideWindow();
            };
            storyboard.Begin();
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            if (this.Closed != null)
            {
                this.Closed(this, new PopupEventArgs());
            }
        }
    }
}
