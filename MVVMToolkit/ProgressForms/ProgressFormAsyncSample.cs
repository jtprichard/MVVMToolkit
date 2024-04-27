using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PB.MVVMToolkit.ProgressForms
{
    public class ProgressFormAsyncSample
    {
        public ProgressFormAsyncSample() { }

        public void Run(int count)
        {
            RunAsyncTask(count);
        }

        public void RunIndeterminate(int count)
        {
            RunAsyncTask(count, true);
        }

        private async void RunAsyncTask(int count, bool isIndeterminate = false)
        {
            var progressData = new ProgressData();
            Func<IProgress<ProgressData>, Task> method = async progress => await AsyncTask(progress, progressData, count);
            ProgressFormAsync pf = new ProgressFormAsync(method, progressData.CancellationTokenSource, "Test Progress Form");
            if(isIndeterminate) pf.Indeterminate = true;
            pf.ShowDialog();
        }

        private async Task<bool> AsyncTask(IProgress<ProgressData> progress, ProgressData progressData, int count)
        {
            progressData.Total = count;
            for (int i = 0; i < count; i++)
            {
                progressData.Message = "Count " + i.ToString();
                progressData.Count = i;
                progressData.GroupMessage = "Group Message";
                progress.Report(progressData);
                await Task.Delay(1000);
            }
            return true;
        }

        private async Task<bool> AsyncWait()
        {
            Thread.Sleep(1000);
            return true;
        }
    }
}
