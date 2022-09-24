using System;
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
using System.IO;

namespace PB.MVVMToolkit.Dialogs
{
    public class ListInputViewModel : BaseDialogViewModel
    {
        #region Private Fields

        private const int DefaultListId = 1000;

        private readonly ObservableCollection<IListItem> _originalItemList = null;
        protected int _listId;
        private List<ListItemProperty> _itemCustomProperties = null;
        private ListItem _dummyListItem;

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

        protected ObservableCollection<ListItem> _visibleListItems;
        /// <summary>
        /// List items used in the listbox user interface.  Note that while these list items may be retrieved outside of the interface,
        /// if there are dependencies, you will only retrieve the list items that are dependent on the combobox when the retrieval is initiated.
        /// List items should generally be retrieved from the ListItems property, which holds all items regardless of dependencies.
        /// </summary>
        public virtual ObservableCollection<ListItem> VisibleListItems
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

        private bool _deleteButtonActive;
        /// <summary>
        /// Enables or disables the delete button on the UI
        /// </summary>
        public bool DeleteButtonActive
        {
            get { return _deleteButtonActive; }
            set { _deleteButtonActive = value; OnPropertyChanged(nameof(DeleteButtonActive)); }
        }

        private bool _modifyButtonActive;
        /// <summary>
        /// Enables or disables the edit button on the UI
        /// </summary>
        public bool ModifyButtonActive
        {
            get { return _modifyButtonActive; }
            set { _modifyButtonActive = value; OnPropertyChanged(nameof(ModifyButtonActive)); }
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
        /// <param name="listItems">List Items as IEnumerable</param>
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
            //If the list item is empty, capture properties from dummy item
            var tempItems = ListItem.Clone(listItems);
            if (tempItems.Count == 0 || (tempItems.Count == 1 && tempItems.FirstOrDefault().Id == -1))
            {
                _dummyListItem = tempItems.FirstOrDefault();
                ListItems = new ObservableCollection<ListItem>();
                _originalItemList = new ObservableCollection<IListItem>();
                _listId = DefaultListId;
            }
            else
            {
                ListItems = ListItem.Clone(listItems);
                _dummyListItem = ListItems.FirstOrDefault();
                _listId = items.Max(x => x.Id);

                //Store the original item list to confirm at the end
                _originalItemList = items;
            }

            //Populate the list and select the first item as default
            if (ComboEnabled)
                PopulateListItemsWithParent();
            else
                PopulateListItems();

            //Store the maximum list id
            //Check if default Id was added, and if so, that it doesn't conflict with existing
            if (defaultId != 0)
            {
                if (items.All(x => x.Id != defaultId))
                {
                    _listId = defaultId - 1;
                }
                else
                {
                    throw new DuplicateNameException("DefaultId value " + defaultId + " is a duplicate of existing ids.");
                }
            }

            //Default to no combobox
            ComboEnabled = false;

            //Default to no Help button
            HelpEnabled = false;

            //Default ItemRequired to false;
            ItemRequired = false;

            //Set instance
            Instance = this;

            UpdateButtons();

        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Show the dialog.
        /// </summary>
        public void Show()
        {
            var view = new ListInputView();
            view.ShowDialog();
        }

        #endregion

        #region Private Methods

        #region Command Events

        /// <summary>
        /// Open the dialog window to add an item
        /// </summary>
        /// <param name="parameter"></param>
        private void OnOpenDialogAdd(object parameter)
        {
            //Create Inputs
            var inputs = CreateDialogInputs();

            //Clear any values from the inputs
            inputs = ClearInputAnswers(inputs);

            string message = "What " + ItemType + " item do you want to add?";
            string caption = "Enter " + ItemType;
            var dialog = new DialogMultiInputOkCancel(message, caption, inputs, DialogImage.Info);
            dialog.Image = DialogImage.Question;

            var result = dialog.Show();

            if (result == DialogResult.Cancel)
            {
                DialogOk.Show("No " + ItemType + " item was added", "Cancel", DialogImage.Info);
                return;
            }

            if (ValidateDialogInputs(inputs))
            {
                _listId++;
                if (VisibleListItems.Any(x => x.Id == _listId))
                {
                    throw new DuplicateNameException("The Id " + _listId + " is duplicated.");
                }
                AddItemToList(inputs, _listId);
            }
        }

        /// <summary>
        /// Open the dialog to edit an item
        /// </summary>
        /// <param name="parameter"></param>
        private void OnOpenDialogEdit(object parameter)
        {
            var selectedItem = SelectedListItem;

            var inputs = CreateDialogInputs(selectedItem);

            string message = "Edit the current " + ItemType + " item";
            string caption = "Edit " + ItemType;

            //var dialog = new DialogInputOkCancel(message, "Edit " + ItemType + " Item");
            var dialog = new DialogMultiInputOkCancel(message, caption, inputs, DialogImage.Info);

            dialog.Image = DialogImage.Question;
            
            var result = dialog.Show();

            if (result == DialogResult.Ok)
            {
                if (ValidateDialogInputs(inputs))
                {
                    EditItemOnList(selectedItem, inputs);
                }
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

        /// <summary>
        /// Command flor clicking the "Help" button
        /// </summary>
        /// <param name="parameter"></param>
        private void OnHelpClicked(object parameter)
        {
            if (File.Exists(HelpFilePath))
            {
                Help.ShowHelp(null, HelpFilePath, HelpTopic);
            }
            else
            {
                DialogOk.Show("No Help File Found", "Error", DialogImage.Error);
            }
        }

        #endregion

        #region Process

        protected ObservableCollection<DialogInput> CreateDialogInputs(ListItem item = null)
        {
            //Create Inputs
            var inputs = new ObservableCollection<DialogInput>();

            //if no list item is provided, get custom properties from other items
            var customProperties = new ObservableCollection<ListItemProperty>();

            if(item == null)
                customProperties = _dummyListItem?.CustomProperties;

            //Else - get them from the provided item
            else
                customProperties = item.CustomProperties;

            AddDescriptionToInputs(item, inputs);

            //Add Custom Properties
            if (customProperties != null)
            {
                foreach (var property in customProperties)
                {
                    AddCustomPropertyToInputs(property, inputs);
                }
            }
            return inputs;
        }

        protected virtual void AddCustomPropertyToInputs(ListItemProperty property, ObservableCollection<DialogInput> inputs)
        {
            if (property.IsLocked != true)
            {
                var customInput = new DialogInput(property.Name, property.Name + ": ");
                customInput.DuplicateAllowed = property.DuplicateAllowed;
                customInput.Required = property.IsRequired;
                customInput.Answer = property.Value;

                inputs.Add(customInput);
            }
        }

        protected virtual void AddDescriptionToInputs(ListItem item, ObservableCollection<DialogInput> inputs)
        {
            //Add Description to Inputs
            var descriptionInput = new DialogInput(nameof(ListItem.Description), ItemType + ": ");
            descriptionInput.DuplicateAllowed = false;
            if (item != null)
            {
                descriptionInput.Answer = item.Description;
                descriptionInput.Id = item.Id;
            }

            inputs.Add(descriptionInput);
        }

        /// <summary>
        /// Validate the input values against duplication and empty
        /// </summary>
        /// <param name="inputs">Dialog Inputs as ObservableCollection</param>
        /// <returns></returns>
        protected bool ValidateDialogInputs(ObservableCollection<DialogInput> inputs)
        {
            bool success = true;

            //Check for empty descriptions

            var descriptionProp = inputs.FirstOrDefault(x => x.Description == nameof(ListItem.Description));
            var descriptionAnswer = descriptionProp?.Answer;

            //Check for duplicates on items
            bool descriptionDuplicated = false;
            string dupItem = string.Empty;

            descriptionDuplicated = VisibleListItems.Any(x => x.Description == descriptionAnswer &&
                                                              x.Id != descriptionProp.Id);

            //Check for duplicate properties
            var dupRestrictedProps = inputs.Where(x => x.DuplicateAllowed == false).ToList();

            foreach (var prop in dupRestrictedProps)
            {
                if (VisibleListItems.Where(x => x.Id != prop.Id).
                    Any(y => y.CustomProperties != null && y.CustomProperties.Any(z => z.Value == prop.Answer)))
                {
                        dupItem = prop.Answer;
                    break;
                }
            }

            //Check for missing required properties
            var requiredPropertyIsEmpty = inputs.Any(x => x.Required && x.Answer == String.Empty);

            if (descriptionDuplicated)
            {
                success = false;
                string errorMsg = "Item must be unique";
                DialogOk.Show(errorMsg, "Error", DialogImage.Error);
            }

            else if (descriptionAnswer == string.Empty)
            {
                success = false;
                string errorMsg = "Value cannot be blank";
                DialogOk.Show(errorMsg, "Error", DialogImage.Error);
            }

            else if (requiredPropertyIsEmpty)
            {
                success = false;
                string errorMsg = "Value cannot be blank";
                DialogOk.Show(errorMsg, "Error", DialogImage.Error);
            }

            else if (dupItem != string.Empty)
            {
                success = false;
                string errorMsg = "Item must be unique";
                DialogOk.Show(errorMsg, "Error", DialogImage.Error);
            }

            return success;
        }

        /// <summary>
        /// Removes an item from the List
        /// </summary>
        /// <param name="item">Listitem object</param>
        private void RemoveItemFromList(ListItem item)
        {
            //Look for the selected item in the list and remove it
            ListItems.Remove(GetSelectedListItem(SelectedListItem));

            UpdateListItems();
        }


        /// <summary>
        /// Adds an item to the list
        /// </summary>
        /// <param name="inputs">User inputs as observablecollection</param>
        /// <param name="id">ListItem Id</param>
        protected void AddItemToList(ObservableCollection<DialogInput> inputs, int id)
        {
            ObservableCollection<ListItemProperty> properties = new ObservableCollection<ListItemProperty>();

            var answer = inputs.FirstOrDefault(x => x.Description == nameof(ListItem.Description))?.Answer;
            var customPropertyInputs = inputs.Where(x => x.Description != nameof(ListItem.Description));
            foreach (var input in customPropertyInputs)
            {
                var property = ListItemProperty.Create(input.Description, input.Answer);
                property.DuplicateAllowed = input.DuplicateAllowed;
                properties.Add(property);
            }

            var newItem = new ListItem(answer, id);
            if (ComboEnabled)
                newItem.Parent = SelectedComboboxItem;
            newItem.AddCustomProperties(properties);

            ListItems.Add(newItem);
            UpdateListItems();
        }

        /// <summary>
        /// Edit an item on the list
        /// </summary>
        /// <param name="item">Listitem object</param>
        /// <param name="inputs">Inputs from user</param>
        protected void EditItemOnList(ListItem item, ObservableCollection<DialogInput> inputs)
        {
            ObservableCollection<ListItemProperty> properties = new ObservableCollection<ListItemProperty>();

            var answer = inputs.FirstOrDefault(x => x.Description == nameof(ListItem.Description))?.Answer;
            var customPropertyInputs = inputs.Where(x => x.Description != nameof(ListItem.Description));
            foreach (var input in customPropertyInputs)
            {
                properties.Add(ListItemProperty.Create(input.Description, input.Answer));
            }

            var selectedItem = GetSelectedListItem(item);
            selectedItem.Description = answer;
            selectedItem.AddCustomProperties(properties);

            UpdateListItems();
        }


        #endregion

        #region Helpers

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
        /// Updates the listitems for re-population
        /// </summary>
        private void UpdateListItems()
        {
            if(ComboEnabled)
                PopulateListItemsWithParent();
            else
                PopulateListItems();
        }

        /// <summary>
        /// Determines a listitems have been edited
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Removes answers from dialog inputs
        /// </summary>
        /// <param name="inputs">DialogInput objects as observable collection</param>
        protected ObservableCollection<DialogInput> ClearInputAnswers(ObservableCollection<DialogInput> inputs)
        {
            foreach (var input in inputs)
            {
                input.Answer = string.Empty;
            }

            return inputs;
        }

        #endregion

        #region Populate

        /// <summary>
        /// Populate the list at startup
        /// </summary>
        private void PopulateListItems()
        {
            var items = new ObservableCollection<ListItem>();

            if (ListItems != null && ListItems.Count!= 0 )
            {
                foreach (var item in ListItems)
                {
                    var newItem = new ListItem(item.Description, item.Id, item.IsLocked);
                    newItem.AddCustomProperties(item.CustomProperties);

                    items.Add(newItem);
                }
            }

            VisibleListItems = items;

            //Adjust the default selecteditem
            if (VisibleListItems.Count > 0)
                SelectedListItem = VisibleListItems.FirstOrDefault();
            else
                SelectedListItem = null;

            UpdateButtons();
        }

        /// <summary>
        /// Populate the list at startup
        /// </summary>
        private void PopulateListItemsWithParent()
        {
            var items = new ObservableCollection<ListItem>();
            foreach (var item in ListItems)
                if (item.Parent.Id == SelectedComboboxItem.Id)
                {
                    var addedItem = new ListItem(item.Description, item.Id, item.IsLocked);
                    addedItem.Parent = item.Parent;
                    items.Add(addedItem);
                }

            VisibleListItems = items;

            //Adjust the default selecteditem
            if (VisibleListItems.Count > 0)
                SelectedListItem = VisibleListItems.FirstOrDefault();
            else
                SelectedListItem = null;

            UpdateButtons();
        }

        /// <summary>
        /// Update the Modify and Delete Button Enabled Property
        /// </summary>
        private void UpdateButtons()
        {
            if (VisibleListItems.Count == 0)
            {
                DeleteButtonActive = false;
                ModifyButtonActive = false;
                return;
            }
            DeleteButtonActive = true;
            ModifyButtonActive = true;

        }
        #endregion

        #endregion

    }
}
