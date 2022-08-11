using System.ComponentModel;

namespace PB.MVVMToolkit.Dialogs.Data
{
    /// <summary>
    /// A class for list item inputs for a multi-input dialog.
    /// </summary>
    public class ListItemProperty : INotifyPropertyChanged
    {
        #region Properties

        /// <summary>
        /// Property Name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Property Value
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// Determines if the property should be locked.
        /// </summary>
        public bool IsLocked { get; set; }
        /// <summary>
        /// Determines if the property should be visible.
        /// </summary>
        public bool IsVisible { get; set; }
        /// <summary>
        /// Notes whether duplicate Values are allowed.
        /// </summary>
        public bool DuplicateAllowed { get; set; }
        /// <summary>
        /// Notes whether an answer Value is required.
        /// </summary>
        public bool IsRequired { get; set; }

        #endregion

        #region Public Methods

        public static ListItemProperty Create(string name, string value, 
            bool isLocked = false, bool isVisible = false, bool isRequired = false)
        {
            var property = new ListItemProperty()
            {
                Name = name,
                Value = value,
                IsLocked = isLocked,
                IsVisible = isVisible,
                IsRequired = isRequired,
                DuplicateAllowed = true
            };
            return property;
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
