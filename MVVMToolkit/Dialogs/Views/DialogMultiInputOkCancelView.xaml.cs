using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PB.MVVMToolkit.Dialogs
{
    /// <summary>
    /// Interaction logic for DialogInputOkCancelView.xaml
    /// </summary>
    internal partial class DialogMultiInputOkCancelView : Window
    {
        internal DialogMultiInputOkCancelView()
        {
            InitializeComponent();
            DataContext = DialogMultiInputOkCancel.Instance;
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            VirtualizingStackPanel.SetIsVirtualizing(lstListView, false);
            ListViewItem item = lstListView.ItemContainerGenerator.ContainerFromIndex(0) as ListViewItem;
            //item.Focus();
            Keyboard.Focus(item);

            //lstListView.SelectAll();
            //lstListView.Focus();

        }
    }
}
