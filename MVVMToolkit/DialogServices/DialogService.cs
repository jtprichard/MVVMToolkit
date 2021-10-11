namespace PB.MVVMToolkit.DialogServices
{
    public class DialogService : IDialogService
    {
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
