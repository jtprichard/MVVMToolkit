using System.IO;
using System.Windows;
using System.Windows.Input;
using PB.MVVMToolkit.DialogServices;
using PB.MVVMToolkit.ViewModel;

namespace PB.MVVMToolkit.Dialogs
{
    /// <summary>
    /// A dialog class to ask a question and return a DialogResult of Yes or No
    /// </summary>
    public class DialogSelectFolder : BaseDialogViewModel
    {
        #region Properties

        /// <summary>
        /// A static reference to the viewmodel
        /// </summary>
        internal static DialogSelectFolder Instance { get; set; }
        /// <summary>
        /// Dialog message as string
        /// </summary>
        public string Message { get; private set; }
        /// <summary>
        /// Dialog caption as string
        /// </summary>
        public string Caption { get; private set; }

        private string _folderPath;
        public string FolderPath
        {
            get => _folderPath;
            set
            {
                if (_folderPath != value)
                {
                    _folderPath = value;
                    OnPropertyChanged(nameof(FolderPath));
                }
            }
        }

        /// <summary>
        /// Dialog Result
        /// Dialog will return Yes or No
        /// </summary>
        private DialogResult Result { get; set; }

        #endregion

        #region Commands

        private ICommand _okCommand = null;
        /// <summary>
        /// Yes Click Command
        /// </summary>
        public ICommand OkCommand
        {
            get { return _okCommand; }
            set { _okCommand = value; }
        }

        private ICommand _cancelCommand = null;
        /// <summary>
        /// No Click Command
        /// </summary>
        public ICommand CancelCommand
        {
            get { return _cancelCommand; }
            set { _cancelCommand = value; }
        }

        private ICommand _browseCommand;
        public ICommand BrowseCommand
        {
            get { return _browseCommand; }
            set { _cancelCommand = value; }
        }

        #endregion

        #region Constructors
        /// <summary>
        /// A dialog asks a question and return a DialogResult of Yes or No
        /// </summary>
        /// <param name="message">Dialog message as string</param>
        /// <param name="caption">Dialog window caption as string</param>
        public DialogSelectFolder(string caption)
        {
            Message = "Select a folder";
            Caption = caption;
            Image = DialogImage.None;

            this._okCommand = new RelayCommand<object>(OnOkClicked);
            this._cancelCommand = new RelayCommand<object>(OnCancelClicked);
            this._browseCommand = new RelayCommand<object>(OnBrowseFolder);
            Instance = this;
        }

        #endregion

        #region Public Methods
        /// <summary>
        /// A dialog asks a question and return a DialogResult of Yes or No
        /// </summary>
        /// <param name="message">Dialog message as string</param>
        /// <param name="caption">Dialog window caption as string</param>
        /// <returns></returns>
        public static string Show(string caption)
        {
            var vm = new DialogSelectFolder(caption);
            vm.Show();
            if(vm.Result == DialogResult.Ok)
            {
                if(File.Exists(vm.FolderPath)) {return vm.FolderPath; }
                else
                {
                    DialogOk.Show("Invalid folder path", vm.Caption, DialogImage.Error);
                    return string.Empty;
                }
            }
            return string.Empty;
        }

        #endregion

        #region Private Methods
        /// <summary>
        /// Show dialog
        /// </summary>
        private void Show()
        {
            var view = new DialogOkCancelView();
            view.ShowDialog();
        }
        /// <summary>
        /// Yes clicked command event
        /// </summary>
        /// <param name="parameter"></param>
        private void OnOkClicked(object parameter)
        {
            Result = DialogResult.Ok;
            CloseDialog(parameter as Window);
        }
        /// <summary>
        /// No clicked command event
        /// </summary>
        /// <param name="parameter"></param>
        private void OnCancelClicked(object parameter)
        {
            Result = DialogResult.Cancel;
            CloseDialog(parameter as Window);
        }

        private void OnBrowseFolder(object parameter)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                dialog.Description = "Select a folder";
                dialog.SelectedPath = FolderPath;
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    FolderPath = dialog.SelectedPath;
                }
            }
        }

        #endregion

    }
}
