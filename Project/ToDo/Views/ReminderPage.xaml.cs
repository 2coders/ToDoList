﻿using System;
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
using Microsoft.Phone.Scheduler;

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
            DateTime date = (DateTime) this.datePicker.Value;
            DateTime time = (DateTime) this.timePicker.Value;
            DateTime beginTime = date + time.TimeOfDay;
            App.TodoParams.RemindTime = beginTime;
            bool b = (bool) reminderSwitch.IsChecked;
            App.TodoParams.Remind = b;
            SetReminder(beginTime);
            App.ViewModel.SaveChangesToDB();

            NavigationService.GoBack();
        }

        #region set reminder
        private void SetReminder(DateTime beginTime)
        {
            if (beginTime <= DateTime.Now)
            {
                return;
            }

            string name = App.TodoParams.Id.ToString();
            if (ScheduledActionService.Find(name) != null)
            {
                ScheduledActionService.Remove(name);
            }

            Reminder reminder = new Reminder(name)
            {
                Title = App.TodoParams.Title,
                Content = App.TodoParams.Note,
                BeginTime = beginTime,
                RecurrenceType = RecurrenceInterval.None
            };
            ScheduledActionService.Add(reminder);

        }
        #endregion

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