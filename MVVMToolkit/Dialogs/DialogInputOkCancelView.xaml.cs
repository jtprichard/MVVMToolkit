using System.Windows;

namespace PB.MVVMToolkit.Dialogs
{
    /// <summary>
    /// Interaction logic for DialogInputOkCancelView.xaml
    /// </summary>
    public partial class DialogInputOkCancelView : Window
    {
        public DialogInputOkCancelView()
        {
            InitializeComponent();
            DataContext = DialogInputOkCancel.Instance;
        }
    }
}
