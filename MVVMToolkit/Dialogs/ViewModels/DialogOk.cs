using System.Windows;
using System.Windows.Input;
using PB.MVVMToolkit.DialogServices;
using PB.MVVMToolkit.ViewModel;

namespace PB.MVVMToolkit.Dialogs
{
    /// <summary>
    /// A dialog class to provide information
    /// </summary>
    public class DialogOk : BaseDialogViewModel
    {
        #region Properties

        /// <summary>
        /// A static reference to the viewmodel
        /// </summary>
        internal static DialogOk Instance { get; set; }
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

        private ICommand _okCommand = null;
        /// <summary>
        /// Yes Click Command
        /// </summary>
        public ICommand OkCommand
        {
            get { return _okCommand; }
            set { _okCommand = value; }
        }

        #endregion

        #region Constructors
        /// <summary>
        /// A dialog asks a question and return a DialogResult of Yes or No
        /// </summary>
        /// <param name="message">Dialog message as string</param>
        /// <param name="caption">Dialog window caption as string</param>
        private DialogOk(string message, string caption, DialogImage image = DialogImage.None)
        {
            Message = message;
            Caption = caption;
            Image = image;

            this._okCommand = new RelayCommand(OnOkClicked);
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
            var vm = new DialogOk(message, caption, image);
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
            var view = new DialogOkView();
            view.ShowDialog();
        }
        /// <summary>
        /// Ok clicked command event
        /// </summary>
        /// <param name="parameter"></param>
        private void OnOkClicked(object parameter)
        {
            Result = DialogResult.Ok;
            CloseDialog(parameter as Window);
        }

        #endregion

    }
}
