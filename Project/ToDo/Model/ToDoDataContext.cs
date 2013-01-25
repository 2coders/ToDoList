using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.ComponentModel;

namespace ToDo.Model
{
    public class ToDoDataContext : DataContext
    {
        public ToDoDataContext(string connectionString)
            : base(connectionString)
        { }

        // Specify a table for the to-do items.
        public Table<ToDoItem> Items;

    }

    [Table]
    public class ToDoItem : INotifyPropertyChanged, INotifyPropertyChanging
    {
        private int _Id;

        [Column(IsPrimaryKey = true, IsDbGenerated = true, DbType = "INT NOT NULL Identity", CanBeNull = false, AutoSync = AutoSync.OnInsert)]
        public int Id
        {
            get { return _Id; }
            set
            {
                if (_Id != value)
                {
                    NotifyPropertyChanging("Id");
                    _Id = value;
                    NotifyPropertyChanged("Id");
                }

            }
        }

        // 条目title
        private string _title;

        [Column]
        public string Title
        {
            get { return _title; }
            set
            {
                //if (_title != value)
                {
                    NotifyPropertyChanging("Title");
                    _title = value;
                    NotifyPropertyChanged("Title");
                }
            }
        }

        // 条目备注
        private string _note;

        [Column]
        public string Note
        {
            get { return _note; }
            set
            {
                if (_note != value)
                {
                    NotifyPropertyChanging("Note");
                    _note = value;
                    NotifyPropertyChanged("Note");
                }
            }
        }

        // 完成状态
        private bool _isCompleted;

        [Column]
        public bool IsCompleted
        {
            get { return _isCompleted; }
            set
            {
                if (_isCompleted != value)
                {
                    NotifyPropertyChanging("IsCompleted");
                    _isCompleted = value;
                    NotifyPropertyChanged("IsCompleted");
                }
            }
        }

        // 提醒开关标志
        private bool _remind;

        [Column]
        public bool Remind
        {
            get { return _remind; }
            set
            {
                if (_remind != value)
                {
                    NotifyPropertyChanging("Remind");
                    _remind = value;
                    NotifyPropertyChanged("Remind");
                }
            }
        }

        // 优先级
        private int _priority;

        [Column]
        public int Priority
        {
            get { return _priority; }
            set
            {
                if (_priority != value)
                {
                    NotifyPropertyChanging("Priority");
                    _priority = value;
                    NotifyPropertyChanged("Priority");
                }
            }
        }

        // 创建时间
        private DateTime _createTime;

        [Column]
        public DateTime CreateTime
        {
            get { return _createTime; }
            set
            {
                if (_createTime != value)
                {
                    NotifyPropertyChanging("CreateTime");
                    _createTime = value;
                    NotifyPropertyChanged("CreateTime");
                }
            }
        }

        // 提醒时间
        private DateTime _remindTime;

        [Column]
        public DateTime RemindTime
        {
            get { return _remindTime; }
            set
            {
                if (_remindTime != value)
                {
                    NotifyPropertyChanging("RemindTime");
                    _remindTime = value;
                    NotifyPropertyChanged("RemindTime");
                }
            }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;


        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            { 
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void INotifyPropertyChanging(string p)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region INotifyPropertyChanging Members

        public event PropertyChangingEventHandler PropertyChanging;

        // Used to notify that a property is about to change
        private void NotifyPropertyChanging(string propertyName)
        {
            if (PropertyChanging != null)
            {
                PropertyChanging(this, new PropertyChangingEventArgs(propertyName));
            }
        }

        #endregion
    }

 }
