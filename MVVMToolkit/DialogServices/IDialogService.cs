namespace PB.MVVMToolkit.DialogServices
{
    /// <summary>
    /// Dialog Services Interface
    /// </summary>
    public interface IDialogService
    {
        void Register<TViewModel, TView>() where TViewModel : IDialogRequestClose
            //where TView : IDialog
            where TView : IDialog;
        void ShowDialog<TViewModel>(TViewModel viewModel) where TViewModel : IDialogRequestClose;
    }
}
