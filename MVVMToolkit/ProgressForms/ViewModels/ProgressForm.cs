using System;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using PB.MVVMToolkit.Dialogs;
using PB.MVVMToolkit.DialogServices;
using PB.MVVMToolkit.ViewModel;

namespace PB.MVVMToolkit.ProgressForms
{
    /// <summary>
    /// A dialog class to ask a question and return a DialogResult of Yes or No
    /// </summary>
    public class ProgressForm : BaseViewModel, IDisposable
    {
        #region Private Fields

        private bool _abortFlag;
        private ProgressFormView _view;
        private BackgroundWorker _worker;

        #endregion

        #region Properties
        /// <summary>
        /// A static reference to the viewmodel
        /// </summary>
        internal static ProgressForm Instance { get; set; }
        /// <summary>
        /// Window object
        /// </summary>
        public Window Owner { get; set; }

        /// <summary>
        /// Dialog message as string
        /// </summary>
        public string Message { get; private set; }
        /// <summary>
        /// Dialog caption as string
        /// </summary>
        public string Caption { get; private set; }

        private int _pbMinimum;
        /// <summary>
        /// The minimum value to use for the progress bar
        /// </summary>
        public int PBMinimum
        {
            get { return _pbMinimum; }
            set { _pbMinimum = value; OnPropertyChanged(nameof(PBMinimum)); }
        }

        private int _pbMaximum;
        /// <summary>
        /// The maximum value to use for the progress bar
        /// </summary>
        public int PBMaximum
        {
            get { return _pbMaximum; }
            set { value = _pbMaximum; OnPropertyChanged(nameof(PBMaximum)); }
        }

        private double _pbValue;
        /// <summary>
        /// The current value of the progress bar between 0 & 100
        /// </summary>
        public double PBValue
        {
            get { return _pbValue; }
            set { _pbValue = value; OnPropertyChanged(nameof(PBValue)); }
        }

        private int _progress;
        /// <summary>
        /// The current value of the progress bar to increment
        /// </summary>
        private int Progress
        {
            get { return _progress; }
            set
            {
                _progress = value;
                PBValue = ConvertValue(value);
            }
        }


        private string _buttonText;
        /// <summary>
        /// Text for Abort button
        /// </summary>
        public string ButtonText
        {
            get { return _buttonText; }
            set { _buttonText = value; OnPropertyChanged(nameof(ButtonText)); }
        }

        /// <summary>
        /// Dialog Result
        /// </summary>
        private DialogResult Result { get; set; }



        #endregion

        #region Commands

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

        public ProgressForm(string caption, string message, int max)
        {
            Caption = caption;
            Message = message;
            ButtonText = "Abort";
            _abortFlag = false;

            PBMinimum = 0;
            PBMaximum = max;
            PBValue = 0;
            Progress = 0;
            Show();
            //Application.DoEvents();

            this._cancelCommand = new RelayCommand(OnCancelClicked);
            Instance = this;

            _worker = new BackgroundWorker();
            _worker.WorkerReportsProgress = true;
            _worker.DoWork += worker_DoWork;
            _worker.ProgressChanged += worker_ProgressChanged;
            _worker.RunWorkerAsync();

        }


        #endregion

        #region Public Methods


        /// <summary>
        /// Show dialog
        /// </summary>
        public void Show()
        {
            _view = new ProgressFormView();
            _view.Show();
            return;
        }

        /// <summary>
        /// Increments progress bar
        /// </summary>
        public void Increment()
        {
            ++Progress;
            _worker.ReportProgress(50);

            //if (null != Message)
            //{
            //    label1.Text = string.Format(_format, progressBar1.Value);
            //}
            //Application.DoEvents();
        }


        /// <summary>
        /// Returns the abort button status
        /// </summary>
        /// <returns>True if abort button clicked</returns>
        public bool GetAbortFlag()
        {
            return _abortFlag;
        }


        #endregion

        #region Private Methods

        private double ConvertValue(double value)
        {
            return (100.0 / PBMaximum) * value;
        }


        /// <summary>
        /// Cancel clicked command event
        /// </summary>
        /// <param name="parameter"></param>
        private void OnCancelClicked(object parameter)
        {
            _abortFlag = true;
            ButtonText = "Aborting...";
            Result = DialogResult.Cancel;
        }

        /// <summary>
        /// Closes dialog window
        /// </summary>
        /// <param name="view"></param>
        private void CloseDialog(Window view)
        {
            if (view != null)
            {
                view.DialogResult = true;
                view.Close();
            }
        }

        #endregion

        public void Dispose()
        {
            CloseDialog(_view);
        }

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            for (int i = 0; i < 100; i++)
            {
                (sender as BackgroundWorker).ReportProgress(i);
                Thread.Sleep(100);
            }
        }

        void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            PBValue = e.ProgressPercentage;
        }
    }
}
