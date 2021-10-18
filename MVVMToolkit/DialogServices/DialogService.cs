namespace PB.MVVMToolkit.DialogServices
{
    public class DialogService : IDialogService
    {
        private static IDialogService _instance;

        public static IDialogService Instance
        {
            get { return _instance ?? new DialogService(); }
            set { _instance = value; }
        }

        public void ShowDialogModal(DialogViewModelBase vm)
        {
            DialogView v = new DialogView();
            v.Owner = vm.Owner;
            v.DataContext = vm;
            v.ShowDialog();
            v.Owner = null;
        }

    }
}
