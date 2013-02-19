using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Controls.Primitives;
using Microsoft.Phone.Controls;
using ToDo.Utils;
using System;

namespace ToDo.Controls
{
    public class PopupWindow: ContentControl
    {
        public enum PopupWindowBackgroundType
        {
            None, Show, Transluent, Flash
        }

        private System.Windows.Controls.ContentPresenter body;
        private System.Windows.Shapes.Rectangle backgroundRect;
        private Control content;
        private PopupWindowBackgroundType backgroundType = PopupWindowBackgroundType.Transluent;

        private static PopupWindow mWindow = null;

        private PopupWindow()
        {
            //这将类的styleKey设置为MyMessage,这样在模板中的style才能通过TargetType="local:MyMessage"与之相互绑定
            this.DefaultStyleKey = typeof(PopupWindow);
        }

        //重写OnApplyTemplate()方法获取模板样式的子控件
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.body = this.GetTemplateChild("body") as ContentPresenter;
            this.backgroundRect = this.GetTemplateChild("backgroundRect") as Rectangle;
            InitializeMessagePrompt();
        }
        //使用Popup控件来制作弹窗
        internal Popup ChildWindowPopup
        {
            get;
            private set;
        }

        //获取当前应用程序的UI框架PhoneApplicationFrame
        private static PhoneApplicationFrame RootVisual
        {
            get
            {
                return Application.Current == null ? null : Application.Current.RootVisual as PhoneApplicationFrame;
            }
        }

        public PopupWindowBackgroundType BackgroundType
        {
            set
            {
                this.backgroundType = value;
            }
        }

        public static PopupWindow CurrentWindow
        {
            get 
            {
                return PopupWindow.mWindow;
            }
        }



        public bool IsOpen
        {
            get
            {
                return ChildWindowPopup != null && ChildWindowPopup.IsOpen;
            }
        }

        public void Hide(Boolean isCanceled)
        {
            if (this.body != null)
            {
                (content as IPopupedControl).IsCanceled = isCanceled;

                //When backroundType is flash, run animation before hide.
                if (this.backgroundType == PopupWindowBackgroundType.Flash)
                {
                    var storyboard = AnimationUtils.GetStoryboard();
                    AnimationUtils.SetOpacityAnimation(storyboard, this.backgroundRect as FrameworkElement, 0, 0.5);
                    storyboard.Begin();
                    storyboard.Completed += delegate(object sender, EventArgs e)
                    {
                        this.ChildWindowPopup.IsOpen = false;
                    };
                }
                else
                {
                    this.ChildWindowPopup.IsOpen = false;
                }
            }
        }

        public void Show()
        {
            if (this.ChildWindowPopup == null)
            {
                this.ChildWindowPopup = new Popup();
                this.ChildWindowPopup.Child = this;
            }

            if (this.ChildWindowPopup != null && Application.Current.RootVisual != null)
            {
                InitializeMessagePrompt();
                this.ChildWindowPopup.IsOpen = true;
            }

            //When backroundType is flash, start animation.
            if (this.backgroundType == PopupWindowBackgroundType.Flash)
            {
                if (content is IPopupedControl)
                {
                    (content as IPopupedControl).Opened += delegate(object sender, PopupEventArgs e)
                    {
                        var storyboard = AnimationUtils.GetStoryboard();
                        AnimationUtils.SetOpacityAnimation(storyboard, this.backgroundRect as FrameworkElement, 0.9, 0.3);
                        storyboard.Begin();
                    };
                }
                
            }
        }
        //初始化弹窗
        private void InitializeMessagePrompt()
        {
            if (this.body == null)
                return;

            switch(this.backgroundType)
            {
                case PopupWindowBackgroundType.None:
                    this.backgroundRect.Visibility = Visibility.Collapsed;
                    break;
                case PopupWindowBackgroundType.Show:
                    this.backgroundRect.Visibility = Visibility.Visible;
                    this.backgroundRect.Opacity = 1;
                    break;
                case PopupWindowBackgroundType.Transluent:
                    this.backgroundRect.Visibility = Visibility.Visible;
                    this.backgroundRect.Opacity = 0.9;
                    break;
                case PopupWindowBackgroundType.Flash:
                    this.backgroundRect.Visibility = Visibility.Visible;
                    this.backgroundRect.Opacity = 0;
                    break;
            }
            this.backgroundRect.Visibility = (this.backgroundType == PopupWindowBackgroundType.None) ? Visibility.Collapsed : Visibility.Visible;
            
           
            this.body.Content = content;
            //this.Height = 800;
        }


        public static void ShowWindow(Control control)
        {
            PopupWindow.ShowWindow(control, PopupWindowBackgroundType.Transluent);
        }

        public static void ShowWindow(Control control, PopupWindowBackgroundType backgroundType)
        {
            if (mWindow == null)
            {
                mWindow = new PopupWindow { content = control, BackgroundType = backgroundType };
                mWindow.Show();
            }
        }

        public static void HideWindow()
        {
            PopupWindow.HideWindow(false);
        }

        public static void HideWindow(bool isCanceled)
        {
            if (mWindow != null)
            {
                mWindow.Hide(isCanceled);
                mWindow = null;
            }
        }

        public static bool IsShown
        {
            get
            {
                if (mWindow != null)
                {
                    return mWindow.IsOpen;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}