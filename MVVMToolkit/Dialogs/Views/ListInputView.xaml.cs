using System.Windows;


namespace PB.MVVMToolkit.Dialogs
{
    /// <summary>
    /// Interaction logic for ListInputView.xaml
    /// </summary>
    public partial class ListInputView : Window
    {
        public ListInputView()
        {
            InitializeComponent();
            DataContext = ListInputViewModel.Instance;
        }
    }
}
