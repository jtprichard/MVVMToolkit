using System.Windows;
using System.Windows.Input;
using PB.MVVMToolkit.DialogServices;
using PB.MVVMToolkit.ViewModel;

namespace PB.MVVMToolkit.Dialogs
{
    /// <summary>
    /// A dialog class to ask a question and return a DialogResult of Yes or No
    /// </summary>
    public class DialogYesNoCancel : BaseDialogViewModel
    {
        #region Properties

        /// <summary>
        /// A static reference to the viewmodel
        /// </summary>
        internal static DialogYesNoCancel Instance { get; set; }
        /// <summary>
        /// Dialog message as string
        /// </summary>
        public string Message { get; private set; }
        /// <summary>
        /// Dialog caption as string
        /// </summary>
        public string Caption { get; private set; }

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

        private ICommand cancelCommand = null;
        /// <summary>
        /// Cancel Click Command
        /// </summary>
        public ICommand CancelCommand
        {
            get { return cancelCommand; }
            set { cancelCommand = value; }
        }

        #endregion

        #region Constructors
        /// <summary>
        /// A dialog asks a question and return a DialogResult of Yes, No, or Cancel
        /// </summary>
        /// <param name="message">Dialog message as string</param>
        /// <param name="caption">Dialog window caption as string</param>
        private DialogYesNoCancel(string message, string caption, DialogImage image = DialogImage.None)
        {
            Message = message;
            Caption = caption;
            Image = image;

            this.yesCommand = new RelayCommand(OnYesClicked);
            this.noCommand = new RelayCommand(OnNoClicked);
            this.cancelCommand = new RelayCommand(OnCancelClicked);
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
            var vm = new DialogYesNoCancel(message, caption, image);
            vm.Show();
            return vm.Result;
        }

        #endregion

        #region Private Methods
        /// <summary>
        /// Show dialog
        /// </summary>
        private void Show()
        {
            var view = new DialogYesNoCancelView();
            view.ShowDialog();
        }
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
        /// <summary>
        /// Cancel clicked command event
        /// </summary>
        /// <param name="parameter"></param>
        private void OnCancelClicked(object parameter)
        {
            Result = DialogResult.Cancel;
            CloseDialog(parameter as Window);
        }

        #endregion

    }
}
