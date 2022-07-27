using System.Windows;
using System.Windows.Controls;

namespace PB.MVVMToolkit.DialogServices
{
    /// <summary>
    /// Interface for Dialogs
    /// </summary>
    public interface IDialog
    {
        object DataContext { get; set; }
        bool? DialogResult { get; set; }
        Window Owner { get; set; }
        void Close();
        bool? ShowDialog();
    }
    
}
