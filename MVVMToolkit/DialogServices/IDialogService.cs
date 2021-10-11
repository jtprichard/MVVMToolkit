using MVVMToolkit.DialogServices.ViewModels;

namespace MVVMToolkit.DialogServices
{
    public interface IDialogService
    {
        void ShowDialogModal(DialogViewModelBase vm);
    }
}
