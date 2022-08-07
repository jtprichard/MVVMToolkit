using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using PB.MVVMToolkit.Dialogs.Data;


namespace PB.MVVMToolkit.Dialogs
{
    /// <summary>
    /// Data class for list items used in list related dialogs
    /// </summary>
    public class CustomListItem : INotifyPropertyChanged, IListItem
    {
        #region Properties
        /// <summary>
        /// Item Id as integer
        /// </summary>
        public int Id { get; set; }

        private string _description;
        /// <summary>
        /// Item description
        /// Description is monitored by property changed
        /// </summary>
        public string Description
        {
            get => _description;
            set { _description = value; OnPropertyChanged(nameof(Description)); }
        }

        /// <summary>
        /// Determines if item is locked for deletion
        /// </summary>
        public bool IsLocked { get; set; }

        /// <summary>
        /// Determines if item is dependent on another listitem
        /// </summary>
        public IListItem Dependency { get; set; }

        public ListItemProperty Hex { get; set; }

        #endregion

        #region Constructors
        /// <summary>
        /// Private empty constructor
        /// </summary>
        private CustomListItem() { }

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="description">Item Description</param>
        /// <param name="id">Item id</param>
        /// <param name="isLocked">Locked for deletion that defaults as false</param>
        public CustomListItem(string description, int id, bool isLocked = false)
        {
            Description = description;
            Id = id;
            IsLocked = isLocked;
        }

        #endregion

        #region Methods
        /// <summary>
        /// Population method used for testing
        /// </summary>
        /// <returns></returns>
        public static ObservableCollection<CustomListItem> Populate()
        {
            ObservableCollection<CustomListItem> items = new ObservableCollection<CustomListItem>
            {
                new CustomListItem {Id = 0, Description = "List Item 1"},
                new CustomListItem {Id = 1, Description = "List Item 2"},

            };
            return items;
        }

        public static ObservableCollection<ListItem> Clone(ObservableCollection<ListItem> items)
        {
            var clonedItems = new ObservableCollection<ListItem>();
            foreach (var item in items)
            {
                var newItem = new ListItem(item.Description, item.Id, item.IsLocked);
                newItem.Dependency = item.Dependency;
                clonedItems.Add(newItem);
            }

            return clonedItems;

        }

        #endregion

        #region Property Changed

        /// <summary>
        /// Occurs when a property value changes
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };

        /// <summary>
        /// Property Changed Method
        /// </summary>
        /// <param name="name"></param>
        public void OnPropertyChanged(string name)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        #endregion


    }
}
