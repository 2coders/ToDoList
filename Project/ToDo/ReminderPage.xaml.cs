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

namespace ToDo
{
    public partial class RemiderPage : PhoneApplicationPage
    {
        private int id;

        public RemiderPage()
        {
            InitializeComponent();
            this.DataContext = App.ViewModel;

            id = int.Parse(NavigationContext.QueryString["id"]);
        }

        private void ApplicationBarDone_Click(object sender, EventArgs e)
        {
            string date = this.date_picker.Text;
            string time = this.time_picker.Text;
            DateTime newDateTime = Convert.ToDateTime(date + time);
            App.ViewModel.updateRemindTime(id, newDateTime);
        }

        
    }
}