using System;
using System.Threading.Tasks;
using System.Windows;

namespace PB.MVVMToolkit.ProgressForms
{
    public partial class ProgressView2 : Window, IDisposable
    {
        public bool IsClosed { get; private set; }
        private Task taskDoEvent { get; set; }

        public ProgressView2(string title = "", double maximum = 100)
        {
            InitializeComponent();
            InitializeSize();
            this.Title = title;
            this.progressBar.Maximum = maximum;
            this.Closed += (s, e) =>
            {
                IsClosed = true;
            };
        }

        public void Dispose()
        {
            if (!IsClosed) Close();
        }

        public bool Update(string message, double value = 1.0)
        {
            this.message.Text = message;
            UpdateTaskDoEvent();
            //if (this.progressBar.Value + value >= progressBar.Maximum)
            //{
            //    progressBar.Maximum += value;
            //}
            progressBar.Value += value;
            return IsClosed;
        }

        private void UpdateTaskDoEvent()
        {
            if (taskDoEvent == null) taskDoEvent = GetTaskUpdateEvent();
            if (taskDoEvent.IsCompleted)
            {
                Show();
                DoEvents();
                taskDoEvent = null;
            }
        }

        private Task GetTaskUpdateEvent()
        {
            return Task.Run(async () => { await Task.Delay(500); });
        }

        private void DoEvents()
        {
            System.Windows.Forms.Application.DoEvents();
            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
        }

        private void InitializeSize()
        {
            this.SizeToContent = SizeToContent.WidthAndHeight;
            this.Topmost = true;
            this.ShowInTaskbar = false;
            this.ResizeMode = ResizeMode.NoResize;
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }
    }
}
