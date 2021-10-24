using System;

namespace PB.MVVMToolkit.DialogServices
{
    /// <summary>
    /// Interface for event arguments to close dialog
    /// </summary>
    public interface IDialogRequestClose
    {
        event EventHandler<DialogCloseRequestedEventArgs> CloseRequested;
    }
}
