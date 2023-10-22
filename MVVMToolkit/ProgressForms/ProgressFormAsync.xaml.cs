using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace PB.MVVMToolkit.ProgressForms
{
    /// <summary>
    /// Interaction logic for ProgressForm.xaml
    /// </summary>
    public partial class ProgressFormAsync : Window
    {
        private readonly List<string> _filenames;
        private CancellationTokenSource _cts;
        private IProgressFormCommand _command;
        private Func <IProgress<int>, bool> _action;

        /// <summary>
        /// Event handler when the abort button is clicked
        /// </summary>
        internal event EventHandler AbortButtonClickedEvent = delegate { };

        /// <summary>
        /// Indicates if the progress bar should be shown
        /// </summary>
        public bool ShowProgressBar { get; set; }

        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="fileTransfer">An IFileTransfer object</param>
        /// <param name="filenames">A list of file names</param>
        /// <param name="message">Message to show in the dialog window</param>
        /// <param name="showProgressBar">Indicate whether progress bar should be displayed</param>
        public ProgressFormAsync(IProgressFormCommand command, string message, bool showProgressBar = true)
        {
            _command = command;
            ShowProgressBar = showProgressBar;

            InitializeComponent();

            this.Message.Text = message;
            this.ProgressPercentage.Text = "0%";
        }

        public ProgressFormAsync(Func<IProgress<int>, bool> action, string message, bool showProgressBar = true)
        {
            _action = action;
            ShowProgressBar = showProgressBar;

            InitializeComponent();

            this.Message.Text = message;
            this.ProgressPercentage.Text = "0%";
        }

        /// <summary>
        /// Content rendered event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void ProgressForm_OnContentRendered(object sender, EventArgs e)
        {
            var type = _command.GetType();
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

            var progress = new Progress<int>(percent =>
            {
                ProgressBar.Value = percent;
                ProgressPercentage.Text = percent + "%";
            });

            await Task.Run(() => _command.Execute(progress));
            //await Task.Run(() => _action(progress));

        }

    }
}
