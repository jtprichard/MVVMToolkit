using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using PB.MVVMToolkit.DialogServices;
using PB.MVVMToolkit.ViewModel;

namespace PB.MVVMToolkit.Dialogs
{
    /// <summary>
    /// A dialog class to ask a question and return a DialogResult of Yes or No
    /// </summary>
    public class DialogYesNo : BaseDialogViewModel
    {
        #region Properties

        /// <summary>
        /// A static reference to the viewmodel
        /// </summary>
        internal static DialogYesNo Instance { get; set; }
        /// <summary>
        /// Dialog message as string
        /// </summary>
        public string Message { get; private set; }
        /// <summary>
        /// Dialog caption as string
        /// </summary>
        public string Caption { get; private set; }

        /// <summary>
        /// Notes whether dialog should say "Do Not Show Me Again" checkbox
        /// </summary>
        public bool ShowDisableFuture { get; set; }

        private bool _disableFuture;
        /// <summary>
        /// Value for user to determine whether the DisableFuture flag should be set
        /// </summary>
        public bool DisableFuture
        {
            get => _disableFuture;
            set { _disableFuture = value; OnPropertyChanged(nameof(DisableFuture)); }
        }

        /// <summary>
        /// Dialog Result
        /// Dialog will return Yes or No
        /// </summary>
        private DialogResult Result { get; set; }

        #endregion

        #region Commands

        private ICommand yesCommand = null;
        /// <summary>
        /// Yes Click Command
        /// </summary>
        public ICommand YesCommand
        {
            get { return yesCommand; }
            set { yesCommand = value; }
        }

        private ICommand noCommand = null;
        /// <summary>
        /// No Click Command
        /// </summary>
        public ICommand NoCommand
        {
            get { return noCommand; }
            set { noCommand = value; }
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

        #endregion

        #region Properties

        private bool _helpEnabled;
        /// <summary>
        /// Enables or disables Help button visibilithy
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


        #endregion

        #region Constructors
        /// <summary>
        /// A dialog asks a question and return a DialogResult of Yes or No
        /// </summary>
        /// <param name="message">Dialog message as string</param>
        /// <param name="caption">Dialog window caption as string</param>
        public DialogYesNo(string message, string caption, DialogImage image = DialogImage.None)
        {
            Message = message;
            Caption = caption;
            Image = image;

            this.yesCommand = new RelayCommand(OnYesClicked);
            this.noCommand = new RelayCommand(OnNoClicked);
            this._helpCommand = new RelayCommand(OnHelpClicked);

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
        public static DialogResult Show(string message, string caption, DialogImage image = DialogImage.None)
        {
            var vm = new DialogYesNo(message, caption, image);
            vm.Show();
            return vm.Result;
        }

        /// <summary>
        /// Show dialog
        /// </summary>
        public DialogResult Show()
        {
            var view = new DialogYesNoView();
            view.ShowDialog();
            return Result;
        }


        #endregion

        #region Private Methods
        /// <summary>
        /// Yes clicked command event
        /// </summary>
        /// <param name="parameter"></param>
        private void OnYesClicked(object parameter)
        {
            Result = DialogResult.Yes;
            CloseDialog(parameter as Window);
        }
        /// <summary>
        /// No clicked command event
        /// </summary>
        /// <param name="parameter"></param>
        private void OnNoClicked(object parameter)
        {
            Result = DialogResult.No;
            CloseDialog(parameter as Window);
        }

        private void OnHelpClicked(object parameter)
        {
            if (System.IO.File.Exists(HelpFilePath))
            {
                Help.ShowHelp(null, HelpFilePath, HelpTopic);
            }
            else
            {
                DialogOk.Show("No Help File Found", "Error", DialogImage.Error);
            }
        }


        #endregion

    }
}
