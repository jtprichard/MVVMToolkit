using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace PB.MVVMToolkit.ProgressForms
{
    /// <summary>
    /// Interaction logic for ProgressForm.xaml
    /// This progress form uses multi-thread.  This form will not work for a Revit
    /// application that requires transactions as it separates the Revit UI thread
    /// from the data thread.  Only use for non-Revit transactions.
    /// </summary>
    public partial class ProgressFormAsync : Window
    {
        private CancellationTokenSource _cts;
        private Func<IProgress<ProgressData>, object[], object> _action;
        private object[] _params;
        private object _returnObject;


        /// <summary>
        /// Event handler when the abort button is clicked
        /// </summary>
        internal event EventHandler AbortButtonClickedEvent = delegate { };

        /// <summary>
        /// Maximum value of the progress bar
        /// </summary>
        public int MaxValue { get; set; }

        /// <summary>
        /// Indicates if the progress bar should be shown
        /// </summary>
        public bool ShowProgressBar { get; set; }
        
        /// <summary>
        /// Indicates if progress bar should be indeterminate
        /// </summary>
        public bool IsIndeterminate { get; set; }

        /// <summary>
        /// Indicates if the progress bar should be shown
        /// </summary>
        public bool ShowProgressBarText => ShowProgressBar && !IsIndeterminate;

        /// <summary>
        /// This progress form uses multi-thread.  This form will not work for a Revit
        /// application that requires transactions as it separates the Revit UI thread
        /// from the data thread.  Only use for non-Revit transactions.
        /// </summary>
        /// <param name="fileTransfer">An IFileTransfer object</param>
        /// <param name="filenames">A list of file names</param>
        /// <param name="message">Message to show in the dialog window</param>
        /// <param name="showProgressBar">Indicate whether progress bar should be displayed</param>
        public ProgressFormAsync(string message, int maxValue = 100, bool showProgressBar = true)
        {
            ShowProgressBar = showProgressBar;
            MaxValue = maxValue;

            InitializeComponent();

            Message.Text = message;
            this.Progress.Text = "0%";
        }

         public object Execute(Func<IProgress<ProgressData>, object[], object> action, params object[] parameters)
        {
            _action = action;
            _params = parameters;
            ProgressBar.IsIndeterminate = IsIndeterminate;

            AsyncShowDialog();
            return _returnObject;
        }

         private async void AsyncShowDialog()
         {
             await AsyncShowDialogCommand();
             return;

         }

         private async Task<bool> AsyncShowDialogCommand()
         {
             this.ShowDialog();
             return true;
         }


        /// <summary>
        /// Content rendered event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void ProgressForm_OnContentRendered(object sender, EventArgs e)
        {
            await ExecuteCommand();

            this.Close();

        }

        /// <summary>
        /// Event handler for the abort button click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AbortButton_OnClick(object sender, RoutedEventArgs e)
        {
            _cts.Cancel();

            AbortButtonClickedEvent?.Invoke(this, EventArgs.Empty);
            this.Close();
        }


        /// <summary>
        /// Method to execute command
        /// </summary>
        /// <returns></returns>
        private async Task ExecuteCommand()
        {
            ProgressBar.Value = 0;
            _cts = new CancellationTokenSource();
            
            
            var progress = new Progress<ProgressData>(progressData =>
            {
                if (!IsIndeterminate)
                {
                    var perc = (int)(((double)progressData.Count / (double)MaxValue) * 100);
                    ProgressBar.Value = perc;
                    Progress.Text = perc + "%";
                }

                Message.Text = progressData.Message;
            });

            _returnObject = await Task.Run(() => _action(progress, _params));
        }



    }
}
