using System.ComponentModel;

namespace PB.MVVMToolkit.Dialogs.Data
{
    public class ListItemProperty : INotifyPropertyChanged
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public bool IsLocked { get; set; }
        public bool IsVisible { get; set; }
        public bool DuplicateAllowed { get; set; }

        #region Public Methods

        public static ListItemProperty Create(string name, string value, bool isLocked = false, bool isVisible = false)
        {
            var property = new ListItemProperty()
            {
                Name = name,
                Value = value,
                IsLocked = isLocked,
                IsVisible = isVisible,
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
