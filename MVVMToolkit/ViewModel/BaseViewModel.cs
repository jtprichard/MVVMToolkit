using System.ComponentModel;
using PB.MVVMToolkit.DialogServices;


namespace PB.MVVMToolkit.ViewModel
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        #region Dialog Service

        protected DialogService DialogService => DialogService.Instance?? null;

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
