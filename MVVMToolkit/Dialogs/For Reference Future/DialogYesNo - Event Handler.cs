using System;
using System.Windows;
using System.Windows.Input;
using PB.MVVMToolkit.DialogServices;
using PB.MVVMToolkit.ViewModel;


namespace PB.MVVMToolkit.Dialogs
{
    public class DialogYesNo : DialogViewModelBase
    {
        public static DialogYesNo Instance { get; set; }

        public event EventHandler YesClicked = delegate { };
        public event EventHandler NoClicked = delegate { };

        public string Message { get; private set; }

        private ICommand yesCommand = null;
        public ICommand YesCommand
        {
            get { return yesCommand; }
            set { yesCommand = value; }
        }

        private ICommand noCommand = null;
        public ICommand NoCommand
        {
            get { return noCommand; }
            set { noCommand = value; }
        }

        public DialogYesNo(string message)
        {
            Message = message;
            this.yesCommand = new RelayCommand(OnYesClicked);
            this.noCommand = new RelayCommand(OnNoClicked);

            Instance = this;
        }

        public void Show()
        {
            var view = new DialogYesNoView();
            view.ShowDialog();
        }

        private void OnYesClicked(object parameter)
        {
            this.YesClicked(this, EventArgs.Empty);
            CloseDialog(parameter as Window);
        }

        private void OnNoClicked(object parameter)
        {
            this.NoClicked(this, EventArgs.Empty);
            CloseDialog(parameter as Window);
        }
    }
}
