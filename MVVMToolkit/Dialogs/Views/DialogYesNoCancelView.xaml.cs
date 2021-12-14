using System.Windows;


namespace PB.MVVMToolkit.Dialogs
{
    /// <summary>
    /// Interaction logic for DialogYesNoView.xaml
    /// </summary>
    internal partial class DialogYesNoCancelView : Window
    {
        internal DialogYesNoCancelView()
        {
            InitializeComponent();
            DataContext = DialogYesNoCancel.Instance;
        }
    }
}
