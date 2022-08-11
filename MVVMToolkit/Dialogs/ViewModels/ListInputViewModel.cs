using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using PB.MVVMToolkit.DialogServices;
using PB.MVVMToolkit.ViewModel;
using PB.MVVMToolkit.Dialogs.Data;
using System.Collections.Generic;

namespace PB.MVVMToolkit.Dialogs
{
    public class ListInputViewModel : BaseDialogViewModel
    {
        #region Private Fields

        private readonly ObservableCollection<IListItem> _originalItemList = null;
        private int _listId;
        private List<ListItemProperty> _itemCustomProperties = null;


        #endregion

        #region Event Handlers

        public event EventHandler OkClicked = delegate { };
        public event EventHandler CancelClicked = delegate { };
        public event EventHandler HelpClicked = delegate { };

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

        private ICommand _helpCommand = null;
        /// <summary>
        /// Command for Help Button
        /// </summary>
        public ICommand HelpCommand
        {
            get { return _helpCommand; }
            set { _helpCommand = value; }
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
        /// List items used in the listbox user interface.  Note that while these list items may be retrieved outside of the interface,
        /// if there are dependencies, you will only retrieve the list items that are dependent on the combobox when the retrieval is initiated.
        /// List items should generally be retrieved from the ListItems property, which holds all items regardless of dependencies.
        /// </summary>
        public ObservableCollection<ListItem> VisibleListItems
        {
            get { return _visibleListItems; }
            set { _visibleListItems = value; OnPropertyChanged(nameof(VisibleListItems)); }
        }

        protected ListItem _selectedListItem;
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
        /// Enables or disables combobox element visibility
        /// </summary>
        public bool ComboEnabled
        {
            get { return _comboEnabled; }
            set { _comboEnabled = value; OnPropertyChanged(nameof(ComboEnabled)); }
        }

        private bool _helpEnabled;
        /// <summary>
        /// Enables or disables Help button visibility
        /// </summary>
        public bool HelpEnabled
        {
            get { return _helpEnabled; }
            set { _helpEnabled = value; OnPropertyChanged(nameof(HelpEnabled)); }
        }

        /// <summary>
        /// File path for help file
        /// </summary>
        public string HelpFilePath { get; set; }

        /// <summary>
        /// The URL for the help topic
        /// </summary>
        public string HelpTopic { get; set; }

        private string _message;
        /// <summary>
        /// The message to be displayed as part of the dialog window.
        /// </summary>
        public string Message
        {
            get { return _message; }
            set { _message = value; OnPropertyChanged(nameof(Message)); }
        }

        private string _comboMessage;
        /// <summary>
        /// The message to be displayed next to the combobox list
        /// </summary>
        public string ComboMessage
        {
            get { return _comboMessage; }
            set { _comboMessage = value; OnPropertyChanged(nameof(ComboMessage)); }
        }

        private IEnumerable<IListItem> _comboboxItems;
        /// <summary>
        /// A list of items for the combobox as an Observable Collection.  To be of use, the list items
        /// must include a dependency parameter to associate with this list.
        /// </summary>
        public IEnumerable<IListItem> ComboboxItems
        {
            get { return _comboboxItems; }
            set { _comboboxItems = value; OnPropertyChanged(nameof(ComboboxItems)); }
        }

        private IListItem _selectedComboboxItem;
        /// <summary>
        /// The selected combobox item.
        /// </summary>
        public IListItem SelectedComboboxItem
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
        /// Item type description that the dialog is working with
        /// </summary>
        public string ItemType { get;}
        /// <summary>
        /// Dialog window title
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Property to determine whether you must always have one item in the list.
        /// The default is false;
        /// </summary>
        public bool ItemRequired { get; set; }
        /// <summary>
        /// View's Owner
        /// </summary>
        public Window Owner { get; set; }

        #endregion

        #region Constructors
        public ListInputViewModel(){}

        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="listItems">List Items as IEnumerable/param>
        /// <param name="message">Dialog message</param>
        /// <param name="itemType">Item type to use in response dialogs</param>
        /// <param name="defaultId">Optional - set the default Id to start with</param>

        public ListInputViewModel(IEnumerable<IListItem> listItems, string message, string itemType, int defaultId = 0)
        {
            //Convert IEnumerable to ObservableCollection
            var items = new ObservableCollection<IListItem>(listItems);

            // Initiate commands
            _openDialogAddCommand = new RelayCommand(OnOpenDialogAdd);
            _openDialogEditCommand = new RelayCommand(OnOpenDialogEdit);
            _openDialogDeleteCommand = new RelayCommand(OnOpenDialogDelete);
            
            this._okCommand = new RelayCommand(OnOkClicked);
            this._cancelCommand = new RelayCommand(OnCancelClicked);
            this._helpCommand = new RelayCommand(OnHelpClicked);

            //Add Message
            Message = message;

            //Add Item Type
            ItemType = itemType;

            //Add window title
            Title = "Edit " + ItemType;

            //Store the full item list regardless of dependency
            ListItems = ListItem.Clone(listItems);

            //Populate the list and select the first item as default
            if (ComboEnabled)
                PopulateListItemsWithDependency();
            else
                PopulateListItems();

            //Store the maximum list id
            //Check if default Id was added, and if so, that it doesn't conflict with existing
            if (defaultId != 0)
            {
                if (items.Count != 0 && items.All(x => x.Id != defaultId))
                {
                    _listId = defaultId - 1;
                }
                else
                {
                    throw new DuplicateNameException("DefaultId value " + defaultId + " is a duplicate of existing ids.");
                }
            }

            //If no defaultId was provided, provide default or locate the maximum
            if (_listId == 0)
            {
                if (items.Count == 0)
                    _listId = 1000;
                else
                    _listId = items.Max(x => x.Id);
            }

            //Store the original item list to confirm at the end
            _originalItemList = items;

            //Default to no combobox
            ComboEnabled = false;

            //Default to no Help button
            HelpEnabled = false;

            //Default ItemRequired to false;
            ItemRequired = false;

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
            //If there are custom properties, then open multi-property box.
            var items = new ObservableCollection<ListItem>(ListItems);

            if(items.FirstOrDefault()?.CustomProperties == null || ListItems.FirstOrDefault().CustomProperties.Count == 0)
                AddSinglePropertyDialog();
            else
                AddMultiPropertyDialog();
        }

        /// <summary>
        /// Open the dialog window to add a property with a single parameter
        /// </summary>
        private void AddSinglePropertyDialog()
        {
            string message = "What " + ItemType + " item do you want to add?";
            var dialog = new DialogInputOkCancel(message, "Add Item");
            dialog.Image = DialogImage.Question;
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
                    if (VisibleListItems.Any(x => x.Id == _listId))
                    {
                        throw new DuplicateNameException("The Id " + _listId + " is duplicated.");
                    }
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
        /// Open the dialog window to add a property with more than one parameter
        /// </summary>
        private void AddMultiPropertyDialog()
        {
            //Create Inputs
            var inputs = new ObservableCollection<DialogInput>();
            var properties = ListItems.FirstOrDefault();
            var customProperties = ListItems.FirstOrDefault().CustomProperties;

            //Add Id and Description to Inputs
            inputs.Add(new DialogInput(nameof(ListItem.Description), ItemType + " Name: ")
            {
                DuplicateAllowed = false
            });

            foreach (var property in customProperties)
            {
                if (property.IsLocked != true)
                {
                    var required = property.IsRequired;
                    inputs.Add(new DialogInput(property.Name, property.Name + ": ")
                    {
                        DuplicateAllowed = property.DuplicateAllowed,
                        Required = required
                    });
                }
            }

            string message = "What " + ItemType + " item do you want to add?";
            string caption = "Enter " + ItemType;
            var dialog = new DialogMultiInputOkCancel(message, caption, inputs, DialogImage.Info);
            dialog.Image = DialogImage.Question;

            var result = dialog.Show();

            if (result == DialogResult.Cancel)
            {
                DialogOk.Show("No " + ItemType + " item was added", "Cancel", DialogImage.Info);
            }
            else
            {
                //Check for duplicate or empty descriptions
                var descriptionAnswer = inputs.FirstOrDefault(x => x.Description == nameof(ListItem.Description))?.Answer;
                var descriptionDuplicated = VisibleListItems.Any(x => x.Description == descriptionAnswer);

                //Check for duplicate properties
                string dupItem = string.Empty;
                var dupRestrictedProps = inputs.Where(x => x.DuplicateAllowed == false).ToList();
                
                foreach (var prop in dupRestrictedProps)
                {
                    if (VisibleListItems.Any(y => y.CustomProperties.Any(z => z.Value == prop.Answer)))
                    {
                        dupItem = prop.Answer;
                        break;
                    }
                }

                //Check for missing required properties
                var requiredPropertyIsEmpty = inputs.Any(x => x.Required && x.Answer == String.Empty);

                if (descriptionDuplicated)
                {
                    string errorMsg = "Item must be unique";
                    DialogOk.Show(errorMsg, "Error", DialogImage.Error);
                }

                else if (descriptionAnswer == string.Empty)
                {
                    string errorMsg = "Value cannot be blank";
                    DialogOk.Show(errorMsg, "Error", DialogImage.Error);
                }

                else if (requiredPropertyIsEmpty)
                {
                    string errorMsg = "Value cannot be blank";
                    DialogOk.Show(errorMsg, "Error", DialogImage.Error);
                }

                else if (dupItem != string.Empty)
                {
                    string errorMsg = "Item must be unique";
                    DialogOk.Show(errorMsg, "Error", DialogImage.Error);
                }
                else
                {
                    _listId++;
                    if (VisibleListItems.Any(x => x.Id == _listId))
                    {
                        throw new DuplicateNameException("The Id " + _listId + " is duplicated.");
                    }
                    AddItemToList(inputs, _listId);
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
            var item = GetSelectedListItem(SelectedListItem);

            //Confirm item is not locked
            if (GetSelectedListItem(SelectedListItem).IsLocked)
            {
                string dialogMessage = SelectedListItem.Description + " is being used and cannot be deleted";
                var deleteErrorResult = DialogOk.Show(dialogMessage, "Error", DialogImage.Error);
                return;
            }

            //Confirm there are at least 2 items on the list
            if (VisibleListItems.Count <= 1 && ItemRequired)
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

        private void AddItemToList(ObservableCollection<DialogInput> inputs, int id)
        {
            ObservableCollection<ListItemProperty> properties = new ObservableCollection<ListItemProperty>();

            var answer = inputs.FirstOrDefault(x => x.Description == nameof(ListItem.Description))?.Answer;
            var customPropertyInputs = inputs.Where(x => x.Description != nameof(ListItem.Description));
            foreach (var input in customPropertyInputs)
            {
                properties.Add(ListItemProperty.Create(input.Description, input.Answer));
            }

            var newItem = new ListItem(answer, id);
            newItem.AddCustomProperties(properties);

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
            var returnedItem = ListItems.FirstOrDefault(x => x.Id == item.Id);

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
            {
                var newItem = new ListItem(item.Description, item.Id, item.IsLocked);
                newItem.AddCustomProperties(item.CustomProperties);

                items.Add(newItem);
            }

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

        private void OnHelpClicked(object parameter)
        {
            if (System.IO.File.Exists(HelpFilePath))
            {
                Help.ShowHelp(null, HelpFilePath,HelpTopic);
            }
            else
            {
                DialogOk.Show("No Help File Found", "Error", DialogImage.Error);
            }
        }

        private bool VerifyItemsChanged()
        {
            if (_originalItemList.Count != ListItems.Count)
                return true;
            for (int i = 0; i < ListItems.Count; i++)
            {
                if (ListItems[i].Description != _originalItemList[i].Description)
                    return true;
            }

            return false;
        }

        #endregion

    }
}
