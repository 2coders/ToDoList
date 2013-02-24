using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;

namespace ToDo.Views
{
    public partial class Setting : PhoneApplicationPage
    {
        public Setting()
        {
            InitializeComponent();
        }

        private void About_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/AboutPage.xaml", UriKind.Relative));
        }

        private void FeedBack_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/FeedBackPage.xaml", UriKind.Relative));
        }

        private void Rate_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            MarketplaceDetailTask task = new MarketplaceDetailTask();
            task.ContentIdentifier = "c14e93aa-27d7-df11-a844-00237de2db9e"; // 指定应用程序在Marketplace中的ID。如果没有指定应用程序，将显示调用应用程序的详细信息页面。
            task.ContentType = MarketplaceContentType.Applications;
            task.Show();
        }
    }
}