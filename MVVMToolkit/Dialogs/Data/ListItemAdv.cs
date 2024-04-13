using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using PB.MVVMToolkit.Dialogs.Data;


namespace PB.MVVMToolkit.Dialogs
{
    /// <summary>
    /// Data class for list items used in list related dialogs
    /// </summary>
    public class ListItemAdv : INotifyPropertyChanged, IListItemAdv
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
        public IListItemAdv Parent { get; set; }

        public ObservableCollection<ListItemProperty> CustomProperties { get; set; }

        /// <summary>
        /// Flag for the addition, changing, or deleting of a list item
        /// </summary>
        public ModifiedFlag Flag { get; set; }


        #endregion

        #region Constructors
        /// <summary>
        /// Private empty constructor
        /// </summary>
        private ListItemAdv() { }

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="description">Item Description</param>
        /// <param name="id">Item id</param>
        /// <param name="isLocked">Locked for deletion that defaults as false</param>
        public ListItemAdv(string description, int id, bool isLocked = false)
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
        public static ObservableCollection<ListItemAdv> Populate()
        {
            ObservableCollection<ListItemAdv> items = new ObservableCollection<ListItemAdv>
            {
                new ListItemAdv {Id = 0, Description = "List Item 1"},
                new ListItemAdv {Id = 1, Description = "List Item 2"},

            };
            return items;
        }

        public static ObservableCollection<IListItemAdv> Clone(IEnumerable<IListItemAdv> items)
        {
            var clonedItems = new ObservableCollection<IListItemAdv>();
            foreach (var item in items)
            {
                var newItem = new ListItemAdv(item.Description, item.Id, item.IsLocked);
                newItem.Parent = item.Parent;
                newItem.Flag = item.Flag;

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

        /// <summary>
        /// Add custom properties to the ListItem
        /// </summary>
        /// <param name="properties"></param>
        public void AddCustomProperties(ObservableCollection<ListItemProperty> properties)
        {
            CustomProperties = properties;
        }

        /// <summary>
        /// Updates Custom Properties via a list of properties to update
        /// </summary>
        /// <param name="properties">An observable collection of properties to update</param>
        public void UpdateCustomProperties(ObservableCollection<ListItemProperty> properties)
        {
            if (properties == null) return;
            foreach (var property in properties)
            {
                var propToUpdate = CustomProperties.FirstOrDefault(x => x.Name == property.Name);
                if (propToUpdate != null)
                {
                    var index = CustomProperties.IndexOf(propToUpdate);
                    CustomProperties[index] = property;
                }
            }
        }

        /// <summary>
        /// Create an empty listitem to feed into ListInputViewModel
        /// </summary>
        /// <param name="properties"></param>
        /// <returns></returns>
        public static ListItemAdv CreateEmptyListItem()
        {
            var item = new ListItemAdv();
            item.Id = -1;
            return item;
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
