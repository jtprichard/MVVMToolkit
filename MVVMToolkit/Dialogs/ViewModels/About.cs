using System.Windows;
using System.Windows.Input;
using PB.MVVMToolkit.DialogServices;
using PB.MVVMToolkit.ViewModel;

namespace PB.MVVMToolkit.Dialogs
{
    /// <summary>
    /// A viewmodel class for the About View
    /// </summary>
    public class About : BaseDialogViewModel
    {
        private string _answer;

        #region Properties
        /// <summary>
        /// A static reference to the viewmodel
        /// </summary>
        internal static About Instance { get; set; }
        /// <summary>
        /// Product Name as string
        /// </summary>
        public string ProductName { get; private set; }
        /// <summary>
        /// Version Number as string
        /// </summary>
        public string VersionNumber { get; private set; }
        /// <summary>
        /// License holder as string
        /// </summary>
        public string Licensee { get; private set; }
        /// <summary>
        /// Custom Entry #1 Tag
        /// </summary>
        public string CustomTag1 { get; private set; }
        /// <summary>
        /// Custom Entry #1 Value
        /// </summary>
        public string CustomValue1 { get; private set; }
        /// <summary>
        /// Custom Entry #2 Tag
        /// </summary>
        public string CustomTag2 { get; private set; }
        /// <summary>
        /// Custom Entry #2 Value
        /// </summary>
        public string CustomValue2 { get; private set; }
        /// <summary>
        /// Custom Entry #3 Tag
        /// </summary>
        public string CustomTag3 { get; private set; }
        /// <summary>
        /// Custom Entry #3 Value
        /// </summary>
        public string CustomValue3 { get; private set; }

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
        /// <param name="productName">Input productName</param>
        /// <param name="versionNumber">Window caption</param>
        public About(string productName, string versionNumber)
        {
            ProductName = productName;
            VersionNumber = versionNumber;

            this._okCommand = new RelayCommand(OnOkClicked);
            Instance = this;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Show Dialog with Product Name and Version Number only
        /// </summary>
        /// <param name="productName">Product Name as string</param>
        /// <param name="versionNumber">Version Number as string</param>
        /// <returns></returns>
        public static DialogResult Show(string productName, string versionNumber)
        {
            var vm = new About(productName, versionNumber);
            vm.Show();
            return vm.Result;
        }


        /// <summary>
        /// Show dialog
        /// </summary>
        public DialogResult Show()
        {
            var view = new AboutView();
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


        #endregion

    }
}
