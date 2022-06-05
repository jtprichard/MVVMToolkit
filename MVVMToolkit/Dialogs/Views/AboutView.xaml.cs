using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.Design;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PB.MVVMToolkit.Dialogs
{
    /// <summary>
    /// Interaction logic for AboutView.xaml
    /// </summary>
    internal partial class AboutView : Window
    {
        public AboutView()
        {
            InitializeComponent();
            DataContext = About.Instance;
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            //txtAnswer.SelectAll();
            //txtAnswer.Focus();
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            // for .NET Core you need to add UseShellExecute = true
            // see https://docs.microsoft.com/dotnet/api/system.diagnostics.processstartinfo.useshellexecute#property-value
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }
    }
}
