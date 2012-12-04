using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Controls.Primitives;
using Microsoft.Phone.Controls;

namespace MessageControl
{
    public class MyMessage : ContentControl
    {
        private System.Windows.Controls.ContentPresenter body;
        private System.Windows.Shapes.Rectangle backgroundRect;
        private Control content;

        private static MyMessage mWindow = null;

        private MyMessage()
        {
            //这将类的styleKey设置为MyMessage,这样在模板中的style才能通过TargetType="local:MyMessage"与之相互绑定
            this.DefaultStyleKey = typeof(MyMessage);
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
        
        //隐藏弹窗
        public void Hide()
        {
            if (this.body != null)
            {
                //关闭Popup控件
                this.ChildWindowPopup.IsOpen = false;
            }
        }
        //判断弹窗是否打开
        public bool IsOpen
        {
            get
            {
                return ChildWindowPopup != null && ChildWindowPopup.IsOpen;
            }
        }
        //打开弹窗
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
        }
        //初始化弹窗
        private void InitializeMessagePrompt()
        {
            if (this.body == null)
                return;
            this.backgroundRect.Visibility = System.Windows.Visibility.Visible;
            //把模板中得body控件内容赋值为你传过来的控件
            this.body.Content = content;
            this.Height = 800;
        }


        public static void PopupWindow(Control control)
        {
            if (mWindow == null)
            {
                mWindow = new MyMessage { content = control };
                mWindow.Show();

            }
        }

        public static void HideWindow()
        {
            if (mWindow != null)
            {
                mWindow.Hide();
                mWindow = null;
            }
        }
    }
}