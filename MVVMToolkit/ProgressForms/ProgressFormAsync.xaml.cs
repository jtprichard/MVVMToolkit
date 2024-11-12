using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Visibility = System.Windows.Visibility;

namespace PB.MVVMToolkit.ProgressForms
{
    /// <summary>
    /// A progress bar window created in WPF.  The window allows for a single progress bar to any desired
    /// maximum value.  Values are incremented by 1 or by the value provided.
    /// A message is available to coincide with progress.  A secondary message is available for sequencing
    /// through groups of items.
    /// </summary>
    public partial class ProgressFormAsync : Window, IDisposable
    {
        private Func<IProgress<int>, CancellationToken, Task> _asyncMethodToRun;
        private Func<IProgress<ProgressData>, Task> _asyncRevisedMethodToRun;
        private CancellationTokenSource _cancellationTokenSource;

        /// <summary>
        /// Returns whether the form is closed.
        /// </summary>
        public bool IsClosed { get; private set; }

        private bool _indeterminate;

        /// <summary>
        /// Sets whether the Indeterminate property for the progress bar should be true
        /// </summary>
        public bool Indeterminate
        {
            get { return _indeterminate; }
            set { _indeterminate = value; SetIndeterminate(value); }

        }

        private string _message;

        public string Message
        {
            get { return _message; }
            set { _message = value; SetMessageText(value);
            }
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

        private bool _showProgressBarText;

        /// <summary>
        /// Indicates if the progress bar should be shown
        /// </summary>
        public bool ShowProgressBarText
        {
            get { return _showProgressBarText;}
            set { _showProgressBarText = value; SetProgressBarTextVisibility(value); }
        }


        /// <summary>
        /// ProgressForm constructor takes a window title and maximum value.
        /// Default value for title is blank, and the default maximum is 100.
        /// </summary>
        /// <param name="title">Window title as string</param>
        /// <param name="maximum">Maximum value to increment to as double</param>
        public ProgressFormAsync(Func<IProgress<int>, CancellationToken, Task> asyncMethodToRun, CancellationTokenSource cancellationTokenSource, string title = "", double maximum = 100)
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
            this.MessageTxt.Text = string.Empty;

            Loaded += ProgressWindow_Loaded;
            Closed += (s, e) => IsClosed = true;

        }


        public ProgressFormAsync(Func<IProgress<ProgressData>, Task> asyncMethodToRun, CancellationTokenSource cancellationTokenSource, string title = "", string message = "")
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
            this.MessageTxt.Text = message;

            Loaded += ProgressWindow_Loaded;
            Closed += (s, e) => IsClosed = true;
        }

        private void SetIndeterminate(bool indeterminate)
        {
            if (ProgressBar == null) return;
            ProgressBar.IsIndeterminate = indeterminate;
            ShowProgressBarText = !indeterminate;
        }

        private async void ProgressWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                CancellationToken cancellationToken = CancellationToken.None;

                var progress = new Progress<ProgressData>(value =>
                {
                    ProgressBar.Maximum = value.Total;
                    int percValue = 0;
                    if (ProgressBar.Maximum == 0)
                        percValue = 100;
                    else
                        percValue =  (int)((((double)value.Count) / ProgressBar.Maximum)*100);
                    ProgressBar.Value = value.Count;
                    Progress.Text =  percValue + "%";
                    Message = value.Message;
                    GroupMessage.Text = value.GroupMessage;
                    cancellationToken = value.CancellationToken;
                });

                if(cancellationToken == CancellationToken.None) BtnCancel.Visibility = Visibility.Collapsed;
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

        private void SetMessageText(string value)
        {
            this.MessageTxt.Text = value;
        }

        private void SetProgressBarVisibility(bool visible)
        {
            if (ProgressBar == null) return;
            this.ProgressBar.Visibility = visible ? Visibility.Visible : Visibility.Collapsed;
            ShowProgressBarText = false;
        }

        private void SetProgressBarTextVisibility(bool visible)
        {
            if (ProgressBar == null) return;
            this.Progress.Visibility = visible ? Visibility.Visible : Visibility.Collapsed;
        }

        public void Dispose()
        {
            if (!IsClosed) 
                Close();
        }
        
    }
}
