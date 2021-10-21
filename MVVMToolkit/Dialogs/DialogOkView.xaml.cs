using System.Windows;

namespace PB.MVVMToolkit.Dialogs
{
    /// <summary>
    /// Interaction logic for DialogOkView.xaml
    /// </summary>
    internal partial class DialogOkView : Window
    {
        public DialogOkView()
        {
            InitializeComponent();
            DataContext = DialogOk.Instance;
        }
    }
}
