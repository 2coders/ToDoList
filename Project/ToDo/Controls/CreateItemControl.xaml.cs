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
using ToDo.Model;
using ToDo.Utils;

namespace ToDo.Controls
{
    public partial class CreateItemControl : UserControl, IPopupedControl
    {
        public static String TODAY = AppResources.ExpandHeaderToday;
        public static String TOMORROW = AppResources.ExpandHeaderTomorrow;
        public static String LATER = AppResources.ExpandHeaderLater;

        public event PopupEventHandler Closed;
        public event PopupEventHandler Opened;

        private ToDoItem item = null;

        private string _GroupName = null;
        public string GroupName
        {
            get
            {
                return _GroupName;
            }
            set
            {
                _GroupName = value;
                groupNameTxt.Text = _GroupName;
            }
        }

        public CreateItemControl()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (Opened != null)
            {
                Opened(this, new PopupEventArgs());
            }
            ContentTextBox.Focus();
            ContentTextBox.SelectionStart = ContentTextBox.Text.Length;
            AddNewItem(" ");
        }

        private void ContentTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            String content;
            if ((content = ContentTextBox.Text.Trim()).Length > 0)
            {
                //AddNewItem(content);
                item.Title = ContentTextBox.Text.Trim();
                App.ViewModel.SaveChangesToDB();
                Log.Info(this.ToString(), "new item created");
            }
            else
            {
                App.ViewModel.DeleteToDoItem(item);
            }
            PopupWindow.HideWindow();
        }

        private void AddNewItem(string content)
        {
            item = new ToDoItem();
            item.Title = content;
            item.CreateTime = DateTime.Now;
            item.IsCompleted = false;
            item.Note = "";
            item.Priority = 0;
            item.Remind = false; // 默认提醒关闭

            if (_GroupName != null && _GroupName.Equals(TOMORROW))
            {
                item.RemindTime = DateTime.Now.AddDays(1);
            }
            else if (_GroupName != null && _GroupName.Equals(LATER))
            {
                item.RemindTime = DateTime.Now.AddDays(2);
            }
            else
            {
                item.RemindTime = DateTime.Now;
            }
            
            App.ViewModel.AddToDoItem(item, _GroupName);
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
