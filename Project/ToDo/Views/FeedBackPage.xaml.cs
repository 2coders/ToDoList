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
    public partial class FeedBackPage : PhoneApplicationPage
    {
        public FeedBackPage()
        {
            InitializeComponent();
            InitApplicationBar();
        }

        private void InitApplicationBar()
        {
            this.ApplicationBar.Buttons.Clear();

            ApplicationBarIconButton send = new ApplicationBarIconButton();
            send.IconUri = new Uri("/Images/done.png", UriKind.Relative);
            send.Text = AppResources.Send;
            send.Click += ApplicationBarIconButton_Click;
            this.ApplicationBar.Buttons.Add(send);
        }

        private void ApplicationBarIconButton_Click(object sender, EventArgs e)
        {
            EmailComposeTask task = new EmailComposeTask();
            task.Subject = AppResources.FeedBackEmailSubject;
            task.To = AppResources.FeedBackEmailTo;
            task.Body = FeedContent.Text;
            task.Show();
            //NavigationService.GoBack();
        }
    }
}