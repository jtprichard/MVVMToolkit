using System.IO;
using System.Windows;
using System.Windows.Input;
using Microsoft.VisualBasic.FileIO;
using PB.MVVMToolkit.DialogServices;

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
        public string ProductName { get; }
        /// <summary>
        /// Version Number as string
        /// </summary>
        public string VersionNumber { get; }
        /// <summary>
        /// License holder as string
        /// </summary>
        public string Licensee { get; set; }
        /// <summary>
        /// The copyright year(s)
        /// </summary>
        public string CopyrightYear { get; }
        /// <summary>
        /// The settings to use if there is a logger
        /// </summary>
        public AboutLogSettings LogSettings { get; }
        /// <summary>
        /// The copyright string for the form
        /// </summary>
        public string Copyright => GetCopyright();

        private string _customTag1;
        /// <summary>
        /// Custom Entry #1 Tag
        /// </summary>
        public string CustomTag1
        {
            get { return _customTag1; }
            set { _customTag1 = value; OnPropertyChanged(nameof(Custom1Visible)); }
        }

        /// <summary>
        /// Custom Entry #1 Value
        /// </summary>
        public string CustomValue1 { get; set; }

        private string _customTag2;
        /// <summary>
        /// Custom Entry #2 Tag
        /// </summary>
        public string CustomTag2
        {
            get { return _customTag2; }
            set { _customTag2 = value; OnPropertyChanged(nameof(Custom2Visible)); }
        }

        /// <summary>
        /// Custom Entry #2 Value
        /// </summary>
        public string CustomValue2 { get; set; }

        private string _customTag3;

        /// <summary>
        /// Custom Entry #3 Tag
        /// </summary>
        public string CustomTag3
        {
            get { return _customTag3; }
            set { _customTag3 = value; OnPropertyChanged(nameof(Custom3Visible)); }
        }
        /// <summary>
        /// Custom Entry #3 Value
        /// </summary>
        public string CustomValue3 { get; set; }

        /// <summary>
        /// Sets the visibility of the Custom1 Tag and Value
        /// </summary>
        public bool Custom1Visible => ConfirmVisibility(CustomTag1);

        /// <summary>
        /// Sets the visibility of the Custom2 Tag and Value
        /// </summary>
        public bool Custom2Visible => ConfirmVisibility(CustomTag2);

        /// <summary>
        /// Sets the visibility of the Custom3 Tag and Value
        /// </summary>
        public bool Custom3Visible => ConfirmVisibility(CustomTag3);

        /// <summary>
        /// Sets the visibility of the Logging items
        /// </summary>
        public bool LoggingEnabled { get; }


        private bool _verboseLoggingChecked;
        /// <summary>
        /// Enables or disables Verbose Logging in the logging assembly
        /// </summary>
        public bool VerboseLoggingChecked
        {
            get { return _verboseLoggingChecked; }
            set { _verboseLoggingChecked = value; OnPropertyChanged(nameof(VerboseLoggingChecked)); }
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

        private ICommand _exportLogFileCommand = null;
        /// <summary>
        /// Export the Log File Command
        /// </summary>
        public ICommand ExportLogFileCommand
        {
            get { return _exportLogFileCommand; }
            set { _exportLogFileCommand = value; }
        }

        #endregion

        #region Constructors
        /// <summary>
        /// A dialog asks a question and return a DialogResult of Yes or No
        /// </summary>
        /// <param name="productName">Input productName</param>
        /// <param name="versionNumber">Window caption</param>
        public About(string productName, string versionNumber, string copyrightYear)
        {
            ProductName = productName;
            VersionNumber = versionNumber;
            CopyrightYear = copyrightYear;

            this._okCommand = new RelayCommand<object>(OnOkClicked);
            this._exportLogFileCommand = new RelayCommand(OnExportLogFileClicked);
            Instance = this;

            //Populate Properties
            PopulateProperties();


        }

        /// <summary>
        /// A dialog asks a question and return a DialogResult of Yes or No
        /// </summary>
        /// <param name="productName">Input productName</param>
        /// <param name="versionNumber">Window caption</param>
        public About(string productName, string versionNumber, string copyrightYear, AboutLogSettings logSettings)
        {
            ProductName = productName;
            VersionNumber = versionNumber;
            CopyrightYear = copyrightYear;
            LogSettings = logSettings;
            LoggingEnabled = (logSettings != null);

            this._okCommand = new RelayCommand<object>(OnOkClicked);
            this._exportLogFileCommand = new RelayCommand(OnExportLogFileClicked);
            Instance = this;

            //Populate Properties
            PopulateProperties();


        }


        #endregion

        #region Command Methods

        /// <summary>
        /// Method when Export Log File button is clicked
        /// </summary>
        /// <param name="parameter"></param>
        private void OnExportLogFileClicked()
        {
            ExportLogFile();
        }


        #endregion

        #region Public Methods

        /// <summary>
        /// Show Dialog with Product Name and Version Number only
        /// </summary>
        /// <param name="productName">Product Name as string</param>
        /// <param name="versionNumber">Version Number as string</param>
        /// <returns></returns>
        public static DialogResult Show(string productName, string versionNumber, string copyrightYear)
        {
            var vm = new About(productName, versionNumber, copyrightYear);
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

        private void PopulateProperties()
        {
            PopulateVerboseLogging();
        }

        /// <summary>
        /// Populate the license message
        /// </summary>
        private void PopulateVerboseLogging()
        {
            if (LogSettings != null) VerboseLoggingChecked = LogSettings.VerboseLoggingEnabled;
            //VerboseLoggingChecked = true;
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

        private bool ConfirmVisibility(string value)
        {
            return !string.IsNullOrEmpty(value);
        }

        private string GetCopyright()
        {
            var language = "Copyright © " + CopyrightYear + " Performance BIM.  All Rights Reserved";
            return language;
        }

        /// <summary>
        /// Exports the Log File
        /// </summary>
        private void ExportLogFile()
        {
            try
            {
                var logFileLocation = LogSettings.LogFilePath;
                SaveLogFile(logFileLocation);
                DialogOk.Show("Log File Saved", "Save Log File", DialogImage.Info);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                DialogOk.Show("Log File Not Saved - Contact the Developer", "Save Log Error", DialogImage.Error);
            }
        }

        /// <summary>
        /// Saves the log file to a user selected location
        /// </summary>
        /// <param name="path">Log file path</param>
        static void SaveLogFile(string path)
        {
            var fileName = Path.GetFileName(path);

            if (fileName != null)
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.FileName = fileName;
                saveFileDialog.Filter = "Log File|*.log";
                saveFileDialog.Title = "Save Log File";
                saveFileDialog.ShowDialog();

                if (saveFileDialog.FileName != "")
                {
                    FileSystem.CopyFile(path, saveFileDialog.FileName, true);
                }
            }
            else
            {
                throw new System.IO.FileNotFoundException();
            }
        }


        #endregion

    }

    public class AboutLogSettings
    {
        public bool VerboseLoggingEnabled { get; set; }
        public bool LoggingEnabled { get; set; }
        public string LogFilePath { get; set; }
    }
}
