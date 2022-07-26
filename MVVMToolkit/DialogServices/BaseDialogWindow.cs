using System.Windows;
using System.Windows.Controls;

namespace PB.MVVMToolkit.DialogServices
{
    /// <summary>
    /// Base class for dialog window views
    /// Class inherits IDialog for DialogServices
    /// </summary>
    public class BaseDialogWindow:Window, IDialogWindow
    {
        /// <summary>
        /// Page Owner
        /// </summary>
        //public new Page Owner { get; set; }
        public new Window Owner { get; set; }
    }
}
