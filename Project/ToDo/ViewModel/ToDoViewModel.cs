using System.ComponentModel;
using ToDo.Model;
using System.Collections.ObjectModel;
using System.Linq;

namespace ToDo.ViewModel
{
    public class ToDoViewModel : INotifyPropertyChanged
    {
         // LINQ to SQL data context for the local database.
        private ToDoDataContext toDoDB;

        // Class constructor, create the data context object.
        public ToDoViewModel(string toDoDBConnectionString)
        {
            toDoDB = new ToDoDataContext(toDoDBConnectionString);
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

       

        // Write changes in the data context to the database.
        public void SaveChangesToDB()
        {
            toDoDB.SubmitChanges();
        }

        // Query database and load the collections and list used by the pivot pages.
        public void LoadCollectionsFromDatabase()
        {

            var todayToDoItemsInDB = from ToDoItem todo in toDoDB.Items
                                     where todo.RemindTime < System.DateTime.Today.AddDays(1)
                                     select todo;
            TodayToDoItems = new ObservableCollection<ToDoItem>(todayToDoItemsInDB);

            var tomorrowToDoItemsInDB = from ToDoItem todo in toDoDB.Items
                                        where todo.RemindTime < System.DateTime.Today.AddDays(2)
                                        && todo.RemindTime > System.DateTime.Today.AddDays(1)
                                     select todo;
            TomorrowToDoItems = new ObservableCollection<ToDoItem>(tomorrowToDoItemsInDB);

            var laterToDoItemsInDB = from ToDoItem todo in toDoDB.Items
                                        where todo.RemindTime > System.DateTime.Today.AddDays(2)
                                        select todo;
            LaterToDoItems = new ObservableCollection<ToDoItem>(laterToDoItemsInDB);

            var completedToDoItemsInDB = from ToDoItem todo in toDoDB.Items
                                     where todo.IsCompleted == true
                                     select todo;
            CompletedToDoItems = new ObservableCollection<ToDoItem>(completedToDoItemsInDB);

        }

        // Add a to-do item to the database and collections.
        public void AddToDoItem(ToDoItem newToDoItem)
        {
            // Add a to-do item to the data context.
            toDoDB.Items.InsertOnSubmit(newToDoItem);

            // Save changes to the database.
            toDoDB.SubmitChanges();

            // Add a to-do item to the "all" observable collection.
            TodayToDoItems.Add(newToDoItem);
        }

        // Remove a to-do task item from the database and collections.
        public void DeleteToDoItem(ToDoItem toDoForDelete)
        {

            // Remove the to-do item from the data context.
            toDoDB.Items.DeleteOnSubmit(toDoForDelete);

            // Save changes to the database.
            toDoDB.SubmitChanges();
        }

        public int getIncompletedItemCount()
        {
            var incompletedItemsInDB = from ToDoItem todo in toDoDB.Items
                                     where todo.IsCompleted == false
                                     select todo;
            ObservableCollection<ToDoItem> incompletedItems = new ObservableCollection<ToDoItem>(incompletedItemsInDB);
            if (incompletedItems != null && incompletedItems.Count > 0)
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
