using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using PB.MVVMToolkit.Dialogs.Data;
using PB.MVVMToolkit.DialogServices;
using PB.MVVMToolkit.ViewModel;

namespace PB.MVVMToolkit.Dialogs
{
    /// <summary>
    /// A dialog class to ask a question and return a DialogResult of Yes or No.
    /// This class allows for multiple inputs using a DialogInput Object.
    /// </summary>
    public class DialogMultiInputOkCancel : BaseDialogViewModel
    {
        #region Properties
        /// <summary>
        /// A static reference to the viewmodel
        /// </summary>
        internal static DialogMultiInputOkCancel Instance { get; set; }
        /// <summary>
        /// Dialog message as string
        /// </summary>
        public string Message { get; private set; }

        private ObservableCollection<DialogInput> _inputCollection;
        /// <summary>
        /// A collection of DialogInput objects for caption and answers 
        /// </summary>
        public ObservableCollection<DialogInput> InputCollection
        {
            get { return _inputCollection; }
            set { _inputCollection = value; OnPropertyChanged(nameof(InputCollection)); }
        }

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
        /// <param name="message">Input message</param>
        /// <param name="caption">Window caption</param>
        /// <param name="inputs">Input parameters as an observable collection of DialogInput objects</param>
        /// <param name="image">Dialog Image Object</param>
        public DialogMultiInputOkCancel(string message, string caption, ObservableCollection<DialogInput> inputs, DialogImage image = DialogImage.None)
        {
            Message = message;
            InputCollection = inputs;
            Caption = caption;

            this._okCommand = new RelayCommand<object>(OnOkClicked);
            this._cancelCommand = new RelayCommand<object>(OnCancelClicked);
            Instance = this;
        }


        #endregion

        #region Public Methods

        /// <summary>
        /// Show dialog
        /// </summary>
        public DialogResult Show()
        {
            var view = new DialogMultiInputOkCancelView();
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
            var inputError = ValidateInputs();
            if (inputError != null)
            {
                DialogOk.Show(inputError.Caption + " cannot be empty.", "Subcategory Manager", DialogImage.Error);
                return;
            }

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

        private DialogInput ValidateInputs()
        {
            foreach (var input in InputCollection)
            {
                if (input.Required && string.IsNullOrEmpty(input.Answer))
                    return input;
            }

            return null;
        }


        #endregion

    }
}
