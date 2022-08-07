using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;


namespace PB.MVVMToolkit.Dialogs.Data
{
    /// <summary>
    /// Data class for list items used in list related dialogs
    /// </summary>
    public class NewListItem : INotifyPropertyChanged, IListItem
    {
        #region Properties
        /// <summary>
        /// Item Id as integer
        /// </summary>
        public int Id { get; set; }

        private ObservableCollection<ListItemProperty> _properties;
        /// <summary>
        /// Properties associated with the ListItem as a ListItemProperty object
        /// </summary>
        public ObservableCollection<ListItemProperty> Properties
        {
            get { return _properties; }
            set { _properties = value; OnPropertyChanged(nameof(Properties)); }
        }

        /// <summary>
        /// Item description
        /// Description is monitored by property changed
        /// </summary>
        public string Description
        {
            get => GetProperty(nameof(Description)) != null ?  GetProperty(nameof(Description)).Value : string.Empty;
            set { GetProperty(nameof(Description)).Value = value; OnPropertyChanged(nameof(Description)); }
        }

        /// <summary>
        /// Determines if item is locked for deletion
        /// </summary>
        public bool IsLocked { get; set; }

        /// <summary>
        /// Determines if item is dependent on another listitem
        /// </summary>
        public IListItem Dependency { get; set; }

        #endregion

        #region Constructors
        /// <summary>
        /// Private empty constructor
        /// </summary>
        private NewListItem() { }

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="description">Item Description</param>
        /// <param name="id">Item id</param>
        /// <param name="isLocked">Locked for deletion that defaults as false</param>
        public NewListItem(string description, int id, bool isLocked = false)
        {
            Properties = new ObservableCollection<ListItemProperty>();

            Properties.Add(new ListItemProperty(nameof(Description)){Value = description, IsVisible= true, IsLocked = false});
            Id = id;
            IsLocked = isLocked;
        }

        public NewListItem(string description, int id, IList<ListItemProperty> properties, bool isLocked = false)
        {
            Properties = new ObservableCollection<ListItemProperty>();

            Properties.Add(new ListItemProperty(nameof(Description)) { Value = description, IsVisible = true, IsLocked = false });
            AddProperties(properties);

            Id = id;
            IsLocked = isLocked;

        }

        #endregion

        #region Public Methods
        /// <summary>
        /// Population method used for testing
        /// </summary>
        /// <returns></returns>
        public static ObservableCollection<NewListItem> Populate()
        {
            ObservableCollection<NewListItem> items = new ObservableCollection<NewListItem>
            {
                new NewListItem {Id = 0, Description = "List Item 1"},
                new NewListItem {Id = 1, Description = "List Item 2"},

            };
            return items;
        }

        public static IEnumerable<IListItem> Clone(ObservableCollection<IListItem> items)
        {
            var clonedItems = new ObservableCollection<NewListItem>();
            foreach (var item in items)
            {
                var newItem = new NewListItem(item.Description, item.Id, item.IsLocked);
                newItem.Dependency = item.Dependency;

                //Get Custom Properties
                newItem.Properties = new ObservableCollection<ListItemProperty>();
                var props = item.GetType().GetProperties();
                var filteredProps = props.Where(x => x.PropertyType == typeof(ListItemProperty)).ToList();

                foreach (var prop in filteredProps)
                {
                    var property = item.GetType().GetProperty(prop.Name);
                    var propertyValue = property.GetValue(item, null) as ListItemProperty;
                    newItem.Properties.Add(propertyValue);

                }

                clonedItems.Add(newItem);
            }
            return clonedItems;
        }

        private List<PropertyInfo> GetCustomProperties()
        {
            var props = this.GetType().GetProperties().ToList();
            var filteredProps = props.Where(x => x.PropertyType == typeof(ListItemProperty)).ToList();
            return filteredProps;
        }

        /// <summary>
        /// Creates a Property for the ListItem
        /// </summary>
        /// <param name="name">Property Name</param>
        public void CreateProperty(string name)
        {
            if(!Properties.Any(x => x.Name == name))
                Properties.Add(new ListItemProperty(name));
        }

        /// <summary>
        /// Sets the Property value
        /// </summary>
        /// <param name="property">ListItemProperty Object</param>
        /// <param name="value">Value to set</param>
        public void SetProperty(ListItemProperty property, string value)
        {
            property.Value = value;
        }

        /// <summary>
        /// Sets the Property value based on a property name
        /// </summary>
        /// <param name="propertyName">Property Name as string</param>
        /// <param name="value">Value as string</param>
        public void SetProperty(string propertyName, string value)
        {
            var property = GetProperty(propertyName);
            if (property == null)
                throw new ArgumentNullException("Property Name does not exist");
            property.Value = value;
        }

        #endregion

        #region Private Methods

        private ListItemProperty GetProperty(string property)
        {
            return Properties.FirstOrDefault(x => x.Name == property);
        }

        private void AddProperties(IList<ListItemProperty> properties)
        {
            foreach (var property in properties)
            {
                Properties.Add(new ListItemProperty(property.Name)
                    {Value = property.Value, 
                        IsVisible = property.IsVisible, 
                        IsLocked = property.IsLocked
                    });
            }
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
