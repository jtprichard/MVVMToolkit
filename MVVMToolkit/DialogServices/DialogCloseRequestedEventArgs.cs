using System;

namespace PB.MVVMToolkit.DialogServices
{
    /// <summary>
    /// Event Arguments Class for closing dialogs
    /// </summary>
    public class DialogCloseRequestedEventArgs : EventArgs
    {
        public bool? DialogResult { get; }
        public DialogCloseRequestedEventArgs(bool? dialogResult)
        {
            DialogResult = dialogResult;
        }
    }
}
