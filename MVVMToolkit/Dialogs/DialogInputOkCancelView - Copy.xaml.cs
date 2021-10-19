using System.Windows;

namespace PB.MVVMToolkit.Dialogs
{
    /// <summary>
    /// Interaction logic for DialogInputOkCancelView.xaml
    /// </summary>
    public partial class DialogInputOkCancelView2 : Window
    {
        public DialogInputOkCancelView2()
        {
            InitializeComponent();
            DataContext = DialogInputOkCancel.Instance;
        }
    }
}
