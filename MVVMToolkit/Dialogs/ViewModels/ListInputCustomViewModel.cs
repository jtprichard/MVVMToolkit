using System;
using System.Collections.ObjectModel;
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
    public abstract class ListInputCustomViewModel : BaseDialogViewModel
    {
        #region Private Fields


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
        /// Instance of viewmodel for databinding
        /// </summary>
        internal static ListInputCustomViewModel Instance { get; set; }

        private IList<IListItemCustom> _listItems;

        /// <summary>
        /// List items as observable collection that may be retrieved following edits.
        /// </summary>
        public IList<IListItemCustom> ListItems
        {
            get { return _listItems; }
            set { _listItems = value; OnPropertyChanged(nameof(VisibleListItems)); }
        }

        /// <summary>
        /// Stored original listitems prior to editing
        /// </summary>
        public ObservableCollection<IListItemCustom> OriginalListItems { get; private set; }

        /// <summary>
        /// List items used in the listbox user interface.
        /// </summary>
        public virtual ObservableCollection<IListItemCustom> VisibleListItems => GetVisibleListItems();
        //{
        //    get { return _visibleListItems; }
        //    set { _visibleListItems = value; OnPropertyChanged(nameof(VisibleListItems)); }
        //}

        protected IListItemCustom _selectedListItem;
        /// <summary>
        /// The selected list item
        /// </summary>
        public IListItemCustom SelectedListItem
        {
            get { return _selectedListItem; }
            set
            {
                _selectedListItem = value;
                OnPropertyChanged(nameof(SelectedListItem));
            }
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

        /// <summary>
        /// Item type description that the dialog is working with
        /// </summary>
        public string ItemType { get; set; }

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
        public new Window Owner { get; set; }

        #endregion

        #region Constructors
        public ListInputCustomViewModel(){}

        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="listItems">List Items as IEnumerable</param>
        /// <param name="message">Dialog message</param>
        /// <param name="itemType">Item type to use in response dialogs</param>
        /// <param name="defaultId">Optional - set the default Id to start with</param>
        public ListInputCustomViewModel(IList<IListItemCustom> listItems, string message, string itemType)
        {
            Instance = this;
            InitializeListItems(listItems);
            InitializeCommands();
            InitializeWindowMessages(message, itemType);
            PopulateListItems();
            SetDefaults();
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
        protected abstract void OnOpenDialogAdd(object parameter);

        /// <summary>
        /// Open the dialog to edit an item
        /// </summary>
        /// <param name="parameter"></param>
        protected abstract void OnOpenDialogEdit(object parameter);

        /// <summary>
        /// Open the dialog to delete an item
        /// </summary>
        /// <param name="parameter"></param>
        protected void OnOpenDialogDelete(object parameter)
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
            Refresh();
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

        /// <summary>
        /// Clone the List Item for change comparison at instantiation.
        /// As these will be used for comparison,
        /// a deep clone should be used to avoid returning changed data.
        /// </summary>
        /// <param name="listItems">A list of IListItemCustom objects</param>
        /// <returns>A deep cloned list of IListItemCustom objects</returns>
        protected abstract IList<IListItemCustom> Clone(IList<IListItemCustom> listItems);

        /// <summary>
        /// Removes an item from the List
        /// </summary>
        /// <param name="item">Listitem object</param>
        private void RemoveItemFromList(IListItemCustom item)
        {
            var deletedItem = GetSelectedListItem(item);
            deletedItem.Flag = ModifiedFlag.Deleted;
            UpdateListItems();
        }

        
        #endregion

        #region Helpers

        /// <summary>
        /// Locates the item in the itemlist
        /// </summary>
        /// <param name="item">Item as ListItem</param>
        /// <returns></returns>
        private IListItemCustom GetSelectedListItem(IListItemCustom item)
        {
            return ListItems.FirstOrDefault(x => x.Id.ToString() == item.Id.ToString());

        }

        /// <summary>
        /// Updates the listitems for re-population
        /// </summary>
        protected void UpdateListItems()
        {
            PopulateListItems();
        }

        /// <summary>
        /// Determines a listitems have been edited
        /// </summary>
        /// <returns></returns>
        private bool VerifyItemsChanged()
        {
            if (OriginalListItems.Count != ListItems.Count)
                return true;
            for (int i = 0; i < ListItems.Count; i++)
            {
                if (ListItems[i].Description != OriginalListItems[i].Description)
                    return true;
            }

            return false;
        }

        #endregion

        #region Populate

        /// <summary>
        /// Populate the list at startup
        /// </summary>
        protected void PopulateListItems()
        {
            var items = new ObservableCollection<IListItemCustom>();

            if (ListItems != null && ListItems.Count!= 0 )
            {
                foreach (var item in ListItems)
                {
                    if(item.Flag != ModifiedFlag.Deleted)
                        items.Add(item);
                }
            }

            //VisibleListItems = new ObservableCollection<IListItemCustom>(items.Where(x => !x.IsHidden));

            if (VisibleListItems.Count > 0)
                SelectedListItem = VisibleListItems.FirstOrDefault();
            else
                SelectedListItem = null;

            UpdateButtons();
        }

        private ObservableCollection<IListItemCustom> GetVisibleListItems()
        {
            return new ObservableCollection<IListItemCustom>(ListItems?.Where(x => !x.IsHidden && x.Flag != ModifiedFlag.Deleted) ?? Array.Empty<IListItemCustom>());
        }

        private void SetDefaults()
        {
            HelpEnabled = false;
            ItemRequired = false;
        }

        private void InitializeWindowMessages(string message, string itemType)
        {
            Message = message;
            ItemType = itemType;
            Title = "Edit " + ItemType;
        }

        private void InitializeListItems(IList<IListItemCustom> listItems)
        {
            if (listItems == null) listItems = new List<IListItemCustom>();
            var items = new ObservableCollection<IListItemCustom>(listItems);
            ListItems = items;
            OriginalListItems = new ObservableCollection<IListItemCustom>(Clone(listItems));
        }

        private void InitializeCommands()
        {
            _openDialogAddCommand = new RelayCommand(OnOpenDialogAdd);
            _openDialogEditCommand = new RelayCommand(OnOpenDialogEdit);
            _openDialogDeleteCommand = new RelayCommand(OnOpenDialogDelete);

            this._okCommand = new RelayCommand(OnOkClicked);
            this._cancelCommand = new RelayCommand(OnCancelClicked);
            this._helpCommand = new RelayCommand(OnHelpClicked);
        }

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

        protected virtual void Refresh()
        {
            OnPropertyChanged(nameof(SelectedListItem));
            OnPropertyChanged(nameof(VisibleListItems));
        }


        #endregion

    }
}
