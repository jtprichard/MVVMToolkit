using System.Windows;

namespace PB.MVVMToolkit.Dialogs.Views
{
    /// <summary>
    /// Interaction logic for DialogSelectFolderView.xaml
    /// </summary>
    public partial class DialogSelectFolderView : Window
    {
        public DialogSelectFolderView()
        {
            InitializeComponent();
            DataContext = DialogSelectFolder.Instance;
        }
    }
}
