using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using ToDo.Model;
using System.Diagnostics;

namespace ToDo
{
    public partial class RemiderPage : PhoneApplicationPage
    {

        public RemiderPage()
        {
            InitializeComponent();
            this.DataContext = App.TodoParams;
        }

        private void ApplicationBarDone_Click(object sender, EventArgs e)
        {
            string date = this.datePicker.ValueString;
            string time = this.timePicker.ValueString;
            Debug.WriteLine(date + time);
            DateTime newDateTime = Convert.ToDateTime(date + " "+ time);
            App.TodoParams.RemindTime = newDateTime;
            App.ViewModel.SaveChangesToDB();

            NavigationService.GoBack();
        }

        private void ApplicationBarCancel_Click(object sender, EventArgs e)
        {
            NavigationService.GoBack();
        }

        private void reminderSwitch_Unchecked(object sender, RoutedEventArgs e)
        {
            this.datePicker.IsEnabled = false;
            this.timePicker.IsEnabled = false;
        }

        private void reminderSwitch_Checked(object sender, RoutedEventArgs e)
        {
            this.datePicker.IsEnabled = true;
            this.timePicker.IsEnabled = true;
        }

        
    }
}