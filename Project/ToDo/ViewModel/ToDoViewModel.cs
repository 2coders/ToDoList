using System.ComponentModel;
using ToDo.Model;
using System.Collections.ObjectModel;
using System.Linq;
using System;
using ToDo.Controls;

namespace ToDo.ViewModel
{
    public class ToDoViewModel : INotifyPropertyChanged
    {

         // LINQ to SQL data context for the local database.
        private ToDoDataContext toDoDBContext;

        // Class constructor, create the data context object.
        public ToDoViewModel(string toDoDBConnectionString)
        {
            toDoDBContext = new ToDoDataContext(toDoDBConnectionString);
        }

        // Today to-do items.
        private ObservableCollection<ToDoItem> _todayToDoItems;
        public ObservableCollection<ToDoItem> TodayToDoItems
        {
            get { return _todayToDoItems; }
            set
            {
                _todayToDoItems = value;
                NotifyPropertyChanged("TodayToDoItems");
            }
        }

        // Tomorrow to-do items.
        private ObservableCollection<ToDoItem> _tomorrowToDoItems;
        public ObservableCollection<ToDoItem> TomorrowToDoItems
        {
            get { return _tomorrowToDoItems; }
            set
            {
                _tomorrowToDoItems = value;
                NotifyPropertyChanged("TomorrowToDoItems");
            }
        }

        // Later to-do items.
        private ObservableCollection<ToDoItem> _laterToDoItems;
        public ObservableCollection<ToDoItem> LaterToDoItems
        {
            get { return _laterToDoItems; }
            set
            {
                _laterToDoItems = value;
                NotifyPropertyChanged("LaterToDoItems");
            }
        }

        // Completed to-do items.
        private ObservableCollection<ToDoItem> _completedToDoItems;
        public ObservableCollection<ToDoItem> CompletedToDoItems
        {
            get { return _completedToDoItems; }
            set
            {
                _completedToDoItems = value;
                NotifyPropertyChanged("CompletedToDoItems");
            }
        }


        // Query database and load the collections and list used by the pivot pages.
        public void LoadCollectionsFromDatabase()
        {

            var todayToDoItemsInDB = from ToDoItem todo in toDoDBContext.Items
                                     where todo.RemindTime < System.DateTime.Today.AddDays(1)
                                     && todo.IsCompleted == false
                                     orderby todo.Id descending
                                     select todo;
            TodayToDoItems = new ObservableCollection<ToDoItem>(todayToDoItemsInDB);

            var tomorrowToDoItemsInDB = from ToDoItem todo in toDoDBContext.Items
                                        where todo.RemindTime < System.DateTime.Today.AddDays(2)
                                        && todo.RemindTime > System.DateTime.Today.AddDays(1)
                                        && todo.IsCompleted == false
                                        orderby todo.Id descending
                                        select todo;
            TomorrowToDoItems = new ObservableCollection<ToDoItem>(tomorrowToDoItemsInDB);

            var laterToDoItemsInDB = from ToDoItem todo in toDoDBContext.Items
                                     where todo.RemindTime > System.DateTime.Today.AddDays(2)
                                     && todo.IsCompleted == false
                                     orderby todo.Id descending
                                     select todo;
            LaterToDoItems = new ObservableCollection<ToDoItem>(laterToDoItemsInDB);

            var completedToDoItemsInDB = from ToDoItem todo in toDoDBContext.Items
                                         where todo.IsCompleted == true
                                         orderby todo.Id descending
                                         select todo;
            CompletedToDoItems = new ObservableCollection<ToDoItem>(completedToDoItemsInDB);

        }

        // Add a to-do item to the database and collections.
        public void AddToDoItem(ToDoItem newToDoItem, string group)
        {
            // Add a to-do item to the data context.
            toDoDBContext.Items.InsertOnSubmit(newToDoItem);

            // Save changes to the database.
            toDoDBContext.SubmitChanges();

            if (group == null)
            {
                return;
            }
            if (group.Equals(CreateItemControl.TOMORROW))
            {
                TomorrowToDoItems.Insert(0, newToDoItem);
            }
            else if (group.Equals(CreateItemControl.LATER))
            {
                LaterToDoItems.Insert(0, newToDoItem);
            }
            else // add to today as default
            {
                // Add a to-do item to the "all" observable collection.
                TodayToDoItems.Insert(0, newToDoItem);
            }
        }

        // Remove a to-do task item from the database and collections.
        public void DeleteToDoItem(ToDoItem toDoForDelete)
        {

            // Remove the to-do item from the data context.
            toDoDBContext.Items.DeleteOnSubmit(toDoForDelete);

            // Save changes to the database.
            toDoDBContext.SubmitChanges();

            if (TodayToDoItems.Contains(toDoForDelete))
            {
                TodayToDoItems.Remove(toDoForDelete);
            }
            else if (TomorrowToDoItems.Contains(toDoForDelete))
            {
                TomorrowToDoItems.Remove(toDoForDelete);
            }
            else if (LaterToDoItems.Contains(toDoForDelete))
            {
                LaterToDoItems.Remove(toDoForDelete);
            }
        }

        // Remove completed items from the database and collections.
        public void DeleteAllCompletedItems()
        {
            toDoDBContext.Items.DeleteAllOnSubmit(CompletedToDoItems);
            // Save changes to the database.
            toDoDBContext.SubmitChanges();
            CompletedToDoItems.Clear();
        }

        // Write changes in the data context to the database. 
        public void UpdateToDoItem(ToDoItem newItem)
        {
            ToDoItem oldItem = toDoDBContext.Items.Single(item => item.Id == newItem.Id);
            oldItem.IsCompleted = newItem.IsCompleted;
            oldItem.Note = newItem.Note;
            oldItem.Priority = newItem.Priority;
            oldItem.RemindTime = newItem.RemindTime;
            oldItem.Title = newItem.Title;
            oldItem.Remind = newItem.Remind;
            toDoDBContext.SubmitChanges();
        }

        public void UpdateRemindTime(int id, DateTime newDateTime)
        {
            ToDoItem oldItem = toDoDBContext.Items.Single(item => item.Id == id);
            oldItem.RemindTime = newDateTime;
            toDoDBContext.SubmitChanges();
        }

        public void ChangeCompletedStatus(ToDoItem item, bool completed)
        {
            toDoDBContext.SubmitChanges();

            if (item == null)
            { 
                return;
            }
            if (TodayToDoItems.Contains(item))
            {
                TodayToDoItems.Remove(item);
            }
            else if (TomorrowToDoItems.Contains(item))
            {
                TomorrowToDoItems.Remove(item);
            }
            else if (LaterToDoItems.Contains(item))
            {
                LaterToDoItems.Remove(item);
            }
            CompletedToDoItems.Insert(0, item);
        }

        // Write changes in the data context to the database.
        public void SaveChangesToDB()
        {
            toDoDBContext.SubmitChanges();
        }

        public int getIncompletedItemCount()
        {
            var incompletedItemsInDB = from ToDoItem todo in toDoDBContext.Items
                                       where todo.RemindTime < System.DateTime.Today.AddDays(1)
                                       && todo.IsCompleted == false
                                       select todo;
            ObservableCollection<ToDoItem> incompletedItems = new ObservableCollection<ToDoItem>(incompletedItemsInDB);
            if (incompletedItems != null)
            {
                return incompletedItems.Count;
            }
            return 0;
        }



        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        // Used to notify Silverlight that a property has changed.
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
    }
}
