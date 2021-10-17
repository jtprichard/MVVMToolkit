using System.Windows;


namespace PB.MVVMToolkit.Dialogs
{
    /// <summary>
    /// Interaction logic for DialogYesNoView.xaml
    /// </summary>
    partial class DialogYesNoView : Window
    {
        internal DialogYesNoView()
        {
            InitializeComponent();
            DataContext = DialogYesNo.Instance;
        }
    }
}
