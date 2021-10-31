using System.Collections.ObjectModel;
using System.ComponentModel;


namespace PB.MVVMToolkit.Dialogs
{
    /// <summary>
    /// Data class for list items used in list related dialogs
    /// </summary>
    public class ListItem : INotifyPropertyChanged
    {
        #region Properties
        /// <summary>
        /// Item Id as integer
        /// </summary>
        public int Id { get; private set; }

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
        public ListItem Dependency { get; set; }

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

        #region Methods
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
