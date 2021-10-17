using PB.MVVMToolkit.DialogServices;
using Microsoft.Toolkit.Mvvm.ComponentModel;



namespace PB.MVVMToolkit.ViewModel
{
    public class ViewModelBase : ObservableObject
    {
        private IDialogService _dialogService;

        public IDialogService DialogService
        {
            get => _dialogService;
            set
            {
                _dialogService = value;
                DialogServices.DialogService.Instance = value;
            }
        }


    }
}
