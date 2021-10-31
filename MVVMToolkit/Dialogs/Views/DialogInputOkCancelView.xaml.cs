using System;
using System.Windows;

namespace PB.MVVMToolkit.Dialogs
{
    /// <summary>
    /// Interaction logic for DialogInputOkCancelView.xaml
    /// </summary>
    internal partial class DialogInputOkCancelView : Window
    {
        internal DialogInputOkCancelView()
        {
            InitializeComponent();
            DataContext = DialogInputOkCancel.Instance;
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            txtAnswer.SelectAll();
            txtAnswer.Focus();
        }
    }
}
