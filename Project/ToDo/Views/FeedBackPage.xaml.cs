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