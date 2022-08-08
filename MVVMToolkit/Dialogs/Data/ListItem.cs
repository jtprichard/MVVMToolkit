using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using PB.MVVMToolkit.Dialogs.Data;


namespace PB.MVVMToolkit.Dialogs
{
    /// <summary>
    /// Data class for list items used in list related dialogs
    /// </summary>
    public class ListItem : INotifyPropertyChanged, IListItem
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

        public ObservableCollection<ListItemProperty> CustomProperties { get; set; }

        #endregion

        #region Constructors
        /// <summary>
        /// Private empty constructor
        /// </summary>
        private ListItem() { }

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="description">Item Description</param>
        /// <param name="id">Item id</param>
        /// <param name="isLocked">Locked for deletion that defaults as false</param>
        public ListItem(string description, int id, bool isLocked = false)
        {
            Description = description;
            Id = id;
            IsLocked = isLocked;
        }

        #endregion

        #region Public Methods
        /// <summary>
        /// Population method used for testing
        /// </summary>
        /// <returns></returns>
        public static ObservableCollection<ListItem> Populate()
        {
            ObservableCollection<ListItem> items = new ObservableCollection<ListItem>
            {
                new ListItem {Id = 0, Description = "List Item 1"},
                new ListItem {Id = 1, Description = "List Item 2"},

            };
            return items;
        }

        public static ObservableCollection<ListItem> Clone(IEnumerable<IListItem> items)
        {
            var clonedItems = new ObservableCollection<ListItem>();
            foreach (var item in items)
            {
                var newItem = new ListItem(item.Description, item.Id, item.IsLocked);
                newItem.Dependency = item.Dependency;

                //Get Custom Properties
                newItem.CustomProperties = new ObservableCollection<ListItemProperty>();
                var props = item.GetType().GetProperties();
                var filteredProps = props.Where(x => x.PropertyType == typeof(ListItemProperty)).ToList();

                foreach (var prop in filteredProps)
                {
                    var property = item.GetType().GetProperty(prop.Name);
                    var propertyValue = property.GetValue(item, null) as ListItemProperty;
                    if(propertyValue != null)
                        newItem.CustomProperties.Add(propertyValue);
                }

                clonedItems.Add(newItem);
            }
            return clonedItems;
        }

        #region Private Methods

        #endregion

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
