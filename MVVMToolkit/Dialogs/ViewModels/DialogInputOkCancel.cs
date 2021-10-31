using System.Windows;
using System.Windows.Input;
using PB.MVVMToolkit.DialogServices;
using PB.MVVMToolkit.ViewModel;

namespace PB.MVVMToolkit.Dialogs
{
    /// <summary>
    /// A dialog class to ask a question and return a DialogResult of Yes or No
    /// </summary>
    public class DialogInputOkCancel : BaseDialogViewModel
    {
        private string _answer;

        #region Properties
        /// <summary>
        /// A static reference to the viewmodel
        /// </summary>
        internal static DialogInputOkCancel Instance { get; set; }
        /// <summary>
        /// Dialog message as string
        /// </summary>
        public string Message { get; private set; }
        /// <summary>
        /// Dialog caption as string
        /// </summary>
        public string Caption { get; private set; }

        /// <summary>
        /// Default answer to show in the dialog answer textbox
        /// </summary>
        public string Answer
        {
            get { return _answer; }
            set { _answer = value; OnPropertyChanged(nameof(Answer)); }

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

        #endregion

        #region Constructors
        /// <summary>
        /// A dialog asks a question and return a DialogResult of Yes or No
        /// </summary>
        /// <param name="message">Dialog message as string</param>
        /// <param name="caption">Dialog window caption as string</param>
        public DialogInputOkCancel(string message, string caption, string defaultAnswer = "", DialogImage image = DialogImage.None)
        {
            Message = message;
            Caption = caption;
            Answer = defaultAnswer;
            Image = image;

            this._okCommand = new RelayCommand(OnOkClicked);
            this._cancelCommand = new RelayCommand(OnCancelClicked);
            Instance = this;
        }

        public DialogInputOkCancel(string message, string caption)
        {
            Message = message;
            Caption = caption;
            Answer = "";
            Image = DialogImage.None;

            this._okCommand = new RelayCommand(OnOkClicked);
            this._cancelCommand = new RelayCommand(OnCancelClicked);
            Instance = this;
        }



        #endregion

        #region Public Methods
        public static DialogResult Show(string message, string caption, string defaultAnswer, DialogImage image, out string answer)
        {
            var vm = new DialogInputOkCancel(message, caption, defaultAnswer, image);
            vm.Show();
            answer = vm.Answer;
            return vm.Result;
        }

        public static DialogResult Show(string message, string caption, out string answer)
        {
            var vm = new DialogInputOkCancel(message, caption, "", DialogImage.None);
            vm.Show();
            answer = vm.Answer;
            return vm.Result;
        }

        public static DialogResult Show(string message, string caption, string defaultAnswer, out string answer)
        {
            var vm = new DialogInputOkCancel(message, caption, defaultAnswer, DialogImage.None);
            vm.Show();
            answer = vm.Answer;
            return vm.Result;
        }

        public static DialogResult Show(string message, string caption, DialogImage image, out string answer)
        {
            var vm = new DialogInputOkCancel(message, caption, "", image);
            vm.Show();
            answer = vm.Answer;
            return vm.Result;
        }

        /// <summary>
        /// Show dialog
        /// </summary>
        public DialogResult Show()
        {
            var view = new DialogInputOkCancelView();
            view.ShowDialog();
            return Result;
        }

        #endregion

        #region Private Methods

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

        #endregion

    }
}
