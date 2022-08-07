using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using PB.MVVMToolkit.Annotations;

namespace PB.MVVMToolkit.Dialogs.Data
{
    public class ListItemProperty : INotifyPropertyChanged
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public bool IsLocked { get; set; }
        public bool IsVisible { get; set; }

        public ListItemProperty(string name)
        {
            Name = name;
        }

        public ListItemProperty(string name, string value)
        {
            Name = name;
            Value = value;
        }

        #region Public Methods

        public static ListItemProperty Create(string name, string value, bool isLocked = false, bool isVisible = false)
        {
            var property = new ListItemProperty(name, value);
            property.IsLocked = isLocked;
            property.IsVisible = isVisible;
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
