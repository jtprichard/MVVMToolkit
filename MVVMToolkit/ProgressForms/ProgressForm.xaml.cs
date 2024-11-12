using System;
using System.Threading.Tasks;
using System.Windows;
using PB.MVVMToolkit.Dialogs.Helpers;
using Visibility = System.Windows.Visibility;

namespace PB.MVVMToolkit.ProgressForms
{
    /// <summary>
    /// A progress bar window created in WPF.  The window allows for a single progress bar to any desired
    /// maximum value.  Values are incremented by 1 or by the value provided.
    /// A message is available to coincide with progress.  A secondary message is available for sequencing
    /// through groups of items.
    /// </summary>
    public partial class ProgressForm : Window, IDisposable
    {

        /// <summary>
        /// Returns whether the form is closed.
        /// </summary>
        public bool IsClosed { get; private set; }

        /// <summary>
        /// Event task for incrementing progress bar
        /// </summary>
        private Task taskDoEvent { get; set; }

        /// <summary>
        /// Abort flag turns true if Abort button is clicked
        /// </summary>
        private bool _abortFlag = false;

        /// <summary>
        /// Sets whether the Indeterminate property for the progress bar should be true
        /// </summary>
        public bool Indeterminate { get; set; }


        private bool _showProgressBar;

        /// <summary>
        /// Sets whether the progress bar should be shown
        /// </summary>
        public bool ShowProgressBar
        {
            get { return _showProgressBar; }
            set { _showProgressBar = value; SetProgressBarVisibility(value); }
        }

        /// <summary>
        /// Indicates if the progress bar should be shown
        /// </summary>
        public bool ShowProgressBarText => ShowProgressBar && !Indeterminate;


        /// <summary>
        /// ProgressForm constructor takes a window title and maximum value.
        /// Default value for title is blank, and the default maximum is 100.
        /// </summary>
        /// <param name="title">Window title as string</param>
        /// <param name="maximum">Maximum value to increment to as double</param>
        public ProgressForm(string title = "", double maximum = 100)
        {

            //Set defaults
            ShowProgressBar = true;
            Indeterminate = false;

            InitializeComponent();
            InitializeSize();

            this.Title = title;
            this.ProgressBar.Maximum = maximum;

            //Event handler as window is closing
            this.Closed += (s, e) =>
            {
                IsClosed = true;
            };

            this.Progress.Text = "0%";
        }

        public ProgressForm(bool isIndeterminate, string title = "")
        {
            //Set defaults
            Indeterminate = isIndeterminate;
            ShowProgressBar = true;

            InitializeComponent();
            InitializeSize();
            this.Title = title;


            //Event handler as window is closing
            this.Closed += (s, e) =>
            {
                IsClosed = true;
            };

            this.Progress.Text = "0%";

        }

        /// <summary>
        /// Shows the modeless dialog window and forces a render refresh of the window
        /// </summary>
        public new void Show()
        {
            base.Show();
            this.Refresh();
        }

        /// <summary>
        /// Shows the modeless dialog window and forces a render refresh of the window
        /// and updates the message.
        /// </summary>
        /// <param name="message"></param>
        public void Show(string message)
        {
            this.Message.Text = message;
            this.ProgressBar.Refresh();

            base.Show();
            this.Refresh();
        }

        /// <summary>
        /// Disposes instance.  Ensures window closes if class is disposed.
        /// </summary>
        public void Dispose()
        {
            if (!IsClosed) 
                Close();
        }

        /// <summary>
        /// Updates the value of the progress bar
        /// </summary>
        /// <param name="message">Message to show above the progress bar</param>
        /// <param name="value">Current value as string</param>
        /// <returns>True if window is closing</returns>
        public bool Update(string message, double value = 1.0)
        {
            this.Message.Text = message;
            UpdateTaskDoEvent();

            if (this.ProgressBar.Value + value >= ProgressBar.Maximum)
            {
                ProgressBar.Maximum += value;
            }
            ProgressBar.Value += value;

            if (!Indeterminate)
            {
                var perc = (int)(((double)ProgressBar.Value / (double)ProgressBar.Maximum) * 100);
                Progress.Text = perc + "%";
            }

            return IsClosed;
        }

        /// <summary>
        /// Resets the progress bar to 0.
        /// </summary>
        /// <param name="maximum">Maximum value as double</param>
        /// <param name="message">Message to show above the progress bar</param>
        public void Reset(double maximum = 100, string message = "")
        {
            this.ProgressBar.Maximum = maximum;
            this.Message.Text = message;
            ProgressBar.Value = 0;
        }

        /// <summary>
        /// Sets a group message.  This is intended, through the use of the Reset method, to increment the same
        /// progressbar form through multiple lists.
        /// </summary>
        /// <param name="groupMessage">A group message as a string</param>
        public void SetGroupMessage(string groupMessage)
        {
            this.GroupMessage.Text = groupMessage;
            this.GroupMessage.Height = 20;
        }

        /// <summary>
        /// Returns whether the abort flag has been turned to true
        /// </summary>
        /// <returns>True if abort flag has turned true</returns>
        public bool GetAbortFlag()
        {
            return _abortFlag;
        }

        /// <summary>
        /// Updates the task on the async thread
        /// </summary>
        private void UpdateTaskDoEvent()
        {
            if (taskDoEvent == null) 
                taskDoEvent = GetTaskUpdateEvent();
            if (taskDoEvent.IsCompleted)
            {
                Show();
                DoEvents();
                taskDoEvent = null;
            }
        }

        /// <summary>
        /// Updates the task
        /// </summary>
        /// <returns></returns>
        private Task GetTaskUpdateEvent()
        {
            return Task.Run(async () => { await Task.Delay(50); });
        }

        /// <summary>
        /// Perform the event to update via the Forms class
        /// </summary>
        private void DoEvents()
        {
            System.Windows.Forms.Application.DoEvents();
            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
        }

        /// <summary>
        /// Initialize the window
        /// </summary>
        private void InitializeSize()
        {
            this.SizeToContent = SizeToContent.WidthAndHeight;
            this.Topmost = true;
            this.ShowInTaskbar = false;
            this.ResizeMode = ResizeMode.NoResize;
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        /// <summary>
        /// Event called when abort button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AbortButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.Message.Text = "Aborting...";
            _abortFlag = true;
        }

        private void SetProgressBarVisibility(bool visible)
        {
            if (ProgressBar == null) return;
            this.ProgressBar.Visibility = visible ? Visibility.Visible : Visibility.Collapsed;
        }

        
    }
}
