using System.Windows;

namespace MVVMToolkit.DialogServices
{
    /// <summary>
    /// Base View Model Class to be extended by dialogs
    /// </summary>
    public class DialogViewModelBase
    {
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
