using PB.MVVMToolkit.DialogServices;
using System.ComponentModel;



namespace PB.MVVMToolkit.ViewModel
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        #region Dialog Services

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

        #endregion

        #region PropertyChanged
        /// <summary>
        /// Occurs when a property value changes
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };

        /// <summary>
        /// Property Changed Method
        /// </summary>
        /// <param name="name"></param>
        public void OnPropertyChanged(string name)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        #endregion
    }
}
