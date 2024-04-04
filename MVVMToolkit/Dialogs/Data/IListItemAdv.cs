using System.Collections.ObjectModel;

namespace PB.MVVMToolkit.Dialogs.Data
{
    /// <summary>
    /// Interface that defines a ListItem object for use in the Revit Dialog ListInput View and Viewmodel.
    /// Interface includes flags for added, changed, and deleted list items. 
    /// </summary>
    public interface IListItemAdv
    {
        /// <summary>
        /// Id number as integer
        /// </summary>
        int Id { get; set; }
        /// <summary>
        /// Item description as string
        /// </summary>
        string Description { get; set; }
        /// <summary>
        /// Determines if Item is locked for edit
        /// </summary>
        bool IsLocked { get; set; }
        /// <summary>
        /// Dependency object as IListItem
        /// </summary>
        IListItemAdv Parent { get; set; }

        ObservableCollection<ListItemProperty> CustomProperties { get; set; }
        ListItemFlag Flag { get; set; }

    }
}
