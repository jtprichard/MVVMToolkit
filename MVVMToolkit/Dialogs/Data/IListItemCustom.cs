using System.Collections.ObjectModel;
using System.ComponentModel;

namespace PB.MVVMToolkit.Dialogs.Data
{
    /// <summary>
    /// Interface that defines a ListItem object for use in a customized Revit Dialog ListInput View and Viewmodel.
    /// Unlike the IListItem and IListItemAdv interfaces, the interface does not use a ListItemProperty and list item editing must
    /// adjust the native class element.  
    /// Interface includes flags for added, changed, and deleted list items. 
    /// </summary>
    public interface IListItemCustom : IModified, INotifyPropertyChanged
    {
        /// <summary>
        /// Unique Identifier
        /// </summary>
        object Id { get; }
        
        /// <summary>
        /// Item description as string
        /// </summary>
        string Description { get; set; }

        /// <summary>
        /// Determines if item is locked for deletion
        /// </summary>
        bool IsLocked { get; set; }

        /// <summary>
        /// Determines if item is hidden in editor
        /// </summary>
        bool IsHidden { get; set; }
        
    }


    public interface IListItemCustom<T> : IListItemCustom
    {
        new T Id { get; set; }
    }





}
