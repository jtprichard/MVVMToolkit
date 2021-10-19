using System.Windows;

namespace PB.MVVMToolkit.Dialogs
{
    /// <summary>
    /// Interaction logic for DialogOkCancelView.xaml
    /// </summary>
    internal partial class DialogOkCancelView : Window
    {
        internal DialogOkCancelView()
        {
            InitializeComponent();
            DataContext = DialogOkCancel.Instance;
        }
    }
}
