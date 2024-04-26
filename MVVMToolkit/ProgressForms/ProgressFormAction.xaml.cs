using System;
using System.Reflection.Emit;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using PB.MVVMToolkit.Dialogs.Helpers;

namespace PB.MVVMToolkit.ProgressForms
{
    /// <summary>
    /// A progress bar window created in WPF.  The window allows for a single progress bar to any desired
    /// maximum value.  Values are incremented by 1 or by the value provided.
    /// A message is available to coincide with progress.  A secondary message is available for sequencing
    /// through groups of items.
    /// </summary>
    public partial class ProgressFormAction : Window, IDisposable
    {
        private Func<IProgress<int>, CancellationToken, Task> _asyncMethodToRun;
        private Func<IProgress<ProgressData>, Task> _asyncRevisedMethodToRun;
        private CancellationTokenSource _cancellationTokenSource;

        /// <summary>
        /// Returns whether the form is closed.
        /// </summary>
        public bool IsClosed { get; private set; }

        ///// <summary>
        ///// Event task for incrementing progress bar
        ///// </summary>
        //private Task taskDoEvent { get; set; }

        ///// <summary>
        ///// Abort flag turns true if Abort button is clicked
        ///// </summary>
        //private bool _abortFlag = false;

        private bool _indeterminate;

        /// <summary>
        /// Sets whether the Indeterminate property for the progress bar should be true
        /// </summary>
        public bool Indeterminate
        {
            get { return _indeterminate; }
            set { _indeterminate = value; SetIndeterminate(value); }

        }


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
        public bool ShowProgressBarText { get; set; }


        /// <summary>
        /// ProgressForm constructor takes a window title and maximum value.
        /// Default value for title is blank, and the default maximum is 100.
        /// </summary>
        /// <param name="title">Window title as string</param>
        /// <param name="maximum">Maximum value to increment to as double</param>
        public ProgressFormAction(Func<IProgress<int>, CancellationToken, Task> asyncMethodToRun, CancellationTokenSource cancellationTokenSource, string title = "", double maximum = 100)
        {

            //Set defaults
            ShowProgressBar = true;
            Indeterminate = false;
            ShowProgressBarText = !Indeterminate;
            InitializeComponent();
            InitializeSize();

            this.Title = title;
            this.ProgressBar.Maximum = maximum;
            this._asyncMethodToRun = asyncMethodToRun;
            _cancellationTokenSource = cancellationTokenSource;
            this.Progress.Text = "0%";

            Loaded += ProgressWindow_Loaded;

            //Event handler as window is closing
            this.Closed += (s, e) =>
            {
                IsClosed = true;
            };

        }

        public ProgressFormAction(Func<IProgress<ProgressData>, Task> asyncMethodToRun, CancellationTokenSource cancellationTokenSource, string title = "")
        {

            ShowProgressBar = true;
            Indeterminate = false;
            ShowProgressBarText = !Indeterminate;
            InitializeComponent();
            InitializeSize();

            this.Title = title;
            this._asyncRevisedMethodToRun = asyncMethodToRun;
            _cancellationTokenSource = cancellationTokenSource;
            this.Progress.Text = "0%";

            Loaded += ProgressWindow_Loaded;

            //Event handler as window is closing
            this.Closed += (s, e) =>
            {
                IsClosed = true;
            };

        }

        public void SetIndeterminate(bool indeterminate)
        {
            if (ProgressBar == null) return;
            ProgressBar.IsIndeterminate = indeterminate;
            ShowProgressBarText = !indeterminate;
            //Progress.Visibility = indeterminate ? Visibility.Visible : Visibility.Collapsed;
        }


        private async void ProgressWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                CancellationToken cancellationToken;

                var progress = new Progress<ProgressData>(value =>
                {
                    ProgressBar.Maximum = value.Total;
                    var percValue =  (int)((((double)value.Count) / ProgressBar.Maximum)*100);
                    ProgressBar.Value = value.Count;
                    Progress.Text =  percValue + "%";
                    Message.Text = value.Message;
                    GroupMessage.Text = value.GroupMessage;
                    cancellationToken = value.CancellationToken;
                });

                if(cancellationToken == null) BtnCancel.Visibility = Visibility.Collapsed;
                await _asyncRevisedMethodToRun(progress);


            }
            catch(Exception ex) 
            {
                Console.WriteLine(ex);
            }
            finally
            {
                Dispatcher.Invoke(() => Close());
            }
        }

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
        private void BtnCancel_OnClick(object sender, RoutedEventArgs e)
        {
            _cancellationTokenSource.Cancel();
        }

        private void SetProgressBarVisibility(bool visible)
        {
            if (ProgressBar == null) return;
            this.ProgressBar.Visibility = visible ? Visibility.Visible : Visibility.Collapsed;
        }

        public void Dispose()
        {
            if (!IsClosed) 
                Close();
        }
        
    }
}
