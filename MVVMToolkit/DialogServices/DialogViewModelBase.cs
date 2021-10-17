using System.Windows;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using PB.MVVMToolkit.DialogServices;

namespace PB.MVVMToolkit.DialogServices
{
    /// <summary>
    /// Base View Model Class to be extended by dialogs
    /// </summary>
    public class DialogViewModelBase : ObservableObject
    {

        public IDialogService DialogService
        {
            get => DialogServices.DialogService.Instance;
        }

        /// <summary>
        /// Window object
        /// </summary>
        public Window Owner { get; set; }

        /// <summary>
        /// Closes dialog window
        /// </summary>
        /// <param name="view"></param>
        public void CloseDialog(Window view)
        {
            if (view != null)
                view.DialogResult = true;
        }


    }
}
