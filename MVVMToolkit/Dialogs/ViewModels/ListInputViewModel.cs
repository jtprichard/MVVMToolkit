using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using PB.MVVMToolkit.DialogServices;
using PB.MVVMToolkit.ViewModel;

namespace PB.MVVMToolkit.Dialogs
{
    public class ListInputViewModel : BaseDialogViewModel
    {
        #region Private Fields

        private readonly ObservableCollection<ListItem> _originalItemList = null;
        private int _listId;


        #endregion

        #region Event Handlers

        public event EventHandler OkClicked = delegate { };
        public event EventHandler CancelClicked = delegate { };

        #endregion

        #region Commands

        private ICommand _okCommand = null;
        /// <summary>
        /// Command for Ok Button
        /// </summary>
        public ICommand OkCommand
        {
            get { return _okCommand; }
            set { _okCommand = value; }
        }

        private ICommand _cancelCommand = null;
        /// <summary>
        /// Command for Cancel Button
        /// </summary>
        public ICommand CancelCommand
        {
            get { return _cancelCommand; }
            set { _cancelCommand = value; }
        }

        private ICommand _openDialogAddCommand = null;
        /// <summary>
        /// Command to Add an Item
        /// </summary>
        public ICommand OpenDialogAddCommand
        {
            get { return _openDialogAddCommand; }
            set { _openDialogAddCommand = value; }
        }

        private ICommand _openDialogEditCommand = null;
        /// <summary>
        /// Command to Edit an Item
        /// </summary>
        public ICommand OpenDialogEditCommand
        {
            get { return _openDialogEditCommand; }
            set { _openDialogEditCommand = value; }
        }

        private ICommand _openDialogDeleteCommand = null;
        /// <summary>
        /// Command to Delete an Item
        /// </summary>
        public ICommand OpenDialogDeleteCommand
        {
            get { return _openDialogDeleteCommand; }
            set { _openDialogDeleteCommand = value; }
        }

        #endregion

        #region Properties
        /// <summary>
        /// List items as observable collection that may be retrieved following edits.
        /// </summary>
        public ObservableCollection<ListItem> ListItems { get; private set; }
        /// <summary>
        /// Instance of viewmodel for databinding
        /// </summary>
        internal static ListInputViewModel Instance { get; set; }
        private ObservableCollection<ListItem> _visibleListItems;
        /// <summary>
        /// List items used in the listbox user inteface.  Note that while these list items may be retrieved outside of the interface,
        /// if there are dependencies, you will only retrieve the list items that are dependent on the combobox when the retrieval is initiated.
        /// List items should generally be retrieved from the ListItems property, which holds all items regardless of dependencies.
        /// </summary>
        public ObservableCollection<ListItem> VisibleListItems
        {
            get { return _visibleListItems; }
            set { _visibleListItems = value; OnPropertyChanged(nameof(VisibleListItems)); }
        }

        private ListItem _selectedListItem;
        /// <summary>
        /// The selected list item
        /// </summary>
        public ListItem SelectedListItem
        {
            get { return _selectedListItem; }
            set
            {
                _selectedListItem = value;
                OnPropertyChanged(nameof(SelectedListItem));
            }
        }

        private bool _comboEnabled;
        /// <summary>
        /// Enables or disables combobox element visibilithy
        /// </summary>
        public bool ComboEnabled
        {
            get { return _comboEnabled; }
            set { _comboEnabled = value; OnPropertyChanged(nameof(ComboEnabled)); }
        }

        private string _message;
        /// <summary>
        /// Dialog Message
        /// </summary>
        public string Message
        {
            get { return _message; }
            set { _message = value; OnPropertyChanged(nameof(Message)); }
        }

        private string _comboMessage;
        /// <summary>
        /// Combobox Message
        /// </summary>
        public string ComboMessage
        {
            get { return _comboMessage; }
            set { _comboMessage = value; OnPropertyChanged(nameof(ComboMessage)); }
        }

        private ObservableCollection<ListItem> _comboboxItems;
        /// <summary>
        /// List items as Observable Collection
        /// </summary>
        public ObservableCollection<ListItem> ComboboxItems
        {
            get { return _comboboxItems; }
            set { _comboboxItems = value; OnPropertyChanged(nameof(ComboboxItems)); }
        }

        private ListItem _selectedComboboxItem;
        /// <summary>
        /// The selected list item
        /// </summary>
        public ListItem SelectedComboboxItem
        {
            get { return _selectedComboboxItem; }
            set
            {
                _selectedComboboxItem = value;
                OnPropertyChanged(nameof(SelectedComboboxItem));
                UpdateListItems();
            }
        }
        /// <summary>
        /// Item type that the dialog is working with
        /// </summary>
        public string ItemType { get;}
        /// <summary>
        /// Dialog window title
        /// </summary>
        public string Title { get; set; }


        #endregion

        #region Constructors
        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="items">List Items as observable collection</param>
        /// <param name="message">Dialog message</param>
        /// <param name="itemType">Item type to use in response dialogs</param>
        public ListInputViewModel(ObservableCollection<ListItem> items, string message, string itemType)
        {
            // Initiate commands
            _openDialogAddCommand = new RelayCommand(OnOpenDialogAdd);
            _openDialogEditCommand = new RelayCommand(OnOpenDialogEdit);
            _openDialogDeleteCommand = new RelayCommand(OnOpenDialogDelete);

            this._okCommand = new RelayCommand(OnOkClicked);
            this._cancelCommand = new RelayCommand(OnCancelClicked);

            //Add Message
            Message = message;

            //Add Item Type
            ItemType = itemType;

            //Add window title
            Title = "Edit " + ItemType;

            //Store the full item list regardless of dependency
            ListItems = ListItem.Clone(items);

            //Populate the list and select the first item as default
            if (ComboEnabled)
                PopulateListItemsWithDependency();
            else
                PopulateListItems();

            //Store the maximum list id
            if (items.Count == 0)
                _listId = 1;
            else
                _listId = items.Max(x => x.Id);

            //Store the original item list to confirm at the end
            _originalItemList = items;

            //Default to no combobox
            ComboEnabled = false;

            //Set instance
            Instance = this;

        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Show dialog
        /// </summary>
        public void Show()
        {
            var view = new ListInputView();
            view.ShowDialog();
        }

        #endregion

        #region Private Methods
        /// <summary>
        /// Open the dialog window to add an item
        /// </summary>
        /// <param name="parameter"></param>
        private void OnOpenDialogAdd(object parameter)
        {
            string message = "What " + ItemType +" item do you want to add?";
            var dialog = new DialogInputOkCancel(message, "Add Item");
            dialog.Image = DialogImage.Question;
            dialog.Answer = "Default " + ItemType + " Item";
            var result = dialog.Show();
            string answer = dialog.Answer;

            if (result == DialogResult.Cancel)
            {
                DialogOk.Show("No " + ItemType + " item was added", "Cancel", DialogImage.Info);
            }
            else
            {
                bool answerDuplicated = VisibleListItems.Any(x => x.Description == answer);
                if (!answerDuplicated)
                {
                    _listId++;
                    AddItemToList(answer, _listId);
                }
                else
                {
                    string errorMsg = ItemType + " name must be unique";
                    DialogOk.Show(errorMsg, "Error", DialogImage.Error);
                }

            }
        }

        /// <summary>
        /// Open the dialog to edit an item
        /// </summary>
        /// <param name="parameter"></param>
        private void OnOpenDialogEdit(object parameter)
        {
            string message = "Edit the current " + ItemType +" item";

            var dialog = new DialogInputOkCancel(message, "Edit " + ItemType + " Item");
            dialog.Image = DialogImage.Question;
            dialog.Answer = SelectedListItem.Description;
            var result = dialog.Show();
            string answer = dialog.Answer;

            if (result == DialogResult.Ok)
            {
                EditItemOnList(SelectedListItem,answer);
            }

        }
        /// <summary>
        /// Open the dialog to delete an item
        /// </summary>
        /// <param name="parameter"></param>
        private void OnOpenDialogDelete(object parameter)
        {

            //Confirm item is not locked
            if (GetSelectedListItem(SelectedListItem).IsLocked)
            {
                string dialogMessage = SelectedListItem.Description + " is being used and cannot be deleted";
                var deleteErrorResult = DialogOk.Show(dialogMessage, "Error", DialogImage.Error);
                return;
            }

            //Confirm there are at least 2 items on the list
            if (VisibleListItems.Count <= 1)
            {
                string dialogMessage = "You must always have one " + ItemType + " item in your list";
                var deleteErrorResult = DialogOk.Show(dialogMessage, "Error", DialogImage.Error);
                return;
            }

            string message = "Are you sure you want to delete the " + ItemType + " item " + SelectedListItem.Description;
            DialogResult result = DialogYesNo.Show(message, "Delete " + ItemType + "Item", DialogImage.Warning);

            if (result == DialogResult.Yes)
            {
                RemoveItemFromList(SelectedListItem);
            }
        }

        private void RemoveItemFromList(ListItem item)
        {
            //Look for the selected item in the list and remove it
            ListItems.Remove(GetSelectedListItem(SelectedListItem));

            UpdateListItems();
        }

        /// <summary>
        /// Adds an item to the list
        /// </summary>
        /// <param name="item"></param>
        private void AddItemToList(string item, int id)
        {
            var newItem = new ListItem(item, id);
            if (ComboEnabled)
                newItem.Dependency = SelectedComboboxItem;
            ListItems.Add(newItem);
            UpdateListItems();
        }

        private void EditItemOnList(ListItem item, string description)
        {
            var selectedItem = GetSelectedListItem(item);
            selectedItem.Description = description;
            UpdateListItems();
        }

        /// <summary>
        /// Locates the item in the itemlist
        /// </summary>
        /// <param name="item">Item as ListItem</param>
        /// <returns></returns>
        private ListItem GetSelectedListItem(ListItem item)
        {
            return ListItems.FirstOrDefault(x => x.Id == item.Id);

        }

        /// <summary>
        /// Populate the list at startup
        /// </summary>
        /// <param name="listItems"></param>
        private void PopulateListItems()
        {
            var items = new ObservableCollection<ListItem>();
            foreach (var item in ListItems)
                items.Add(new ListItem(item.Description, item.Id, item.IsLocked));

            VisibleListItems = items;

            //Adjust the default selecteditem
            if (VisibleListItems.Count > 0)
                SelectedListItem = VisibleListItems.FirstOrDefault();
            else
                SelectedListItem = null;

        }

        /// <summary>
        /// Populate the list at startup
        /// </summary>
        /// <param name="listItems"></param>
        private void PopulateListItemsWithDependency()
        {
            var items = new ObservableCollection<ListItem>();
            foreach (var item in ListItems)
                if(item.Dependency.Id == SelectedComboboxItem.Id)
                    items.Add(new ListItem(item.Description, item.Id, item.IsLocked));

            VisibleListItems = items;

            //Adjust the default selecteditem
            if (VisibleListItems.Count > 0)
                SelectedListItem = VisibleListItems.FirstOrDefault();
            else
                SelectedListItem = null;
        }

        private void UpdateListItems()
        {
            if(ComboEnabled)
                PopulateListItemsWithDependency();
            else
                PopulateListItems();
        }


        /// <summary>
        /// Command for clicking Ok on dialog
        /// </summary>
        /// <param name="parameter"></param>
        private void OnOkClicked(object parameter)
        {
            this.OkClicked(this, EventArgs.Empty);
            CloseDialog(parameter as Window);
        }

        /// <summary>
        /// Command for clicking cancel on dialog
        /// </summary>
        /// <param name="parameter"></param>
        private void OnCancelClicked(object parameter)
        {
            if (VerifyItemsChanged())
            {
                var result = DialogYesNo.Show(ItemType + " items have changed.  Are you sure you want to cancel?", "List Items", DialogImage.Warning);
                if (result != DialogResult.Yes)
                    return;
            }

            this.CancelClicked(this, EventArgs.Empty);
            CloseDialog(parameter as Window);
        }

        private bool VerifyItemsChanged()
        {
            if (_originalItemList.Count == 0 || VisibleListItems.Count == 0)
                return true;
            if (_originalItemList.Count != VisibleListItems.Count)
                return true;
            for (int i = 0; i < VisibleListItems.Count; i++)
            {
                if (VisibleListItems[i].Description != _originalItemList[i].Description)
                    return true;
            }

            return false;
        }

        #endregion

    }
}
