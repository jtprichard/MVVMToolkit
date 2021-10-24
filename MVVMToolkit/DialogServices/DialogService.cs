using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace PB.MVVMToolkit.DialogServices
{
    /// <summary>
    /// Dialog Service Class
    /// </summary>
    public class DialogService : IDialogService
    {
        //private readonly Owner;
        private readonly Page _owner;
        public IDictionary<Type, Type> Mappings { get; }

        public DialogService(Page owner)
        {
            this._owner = owner;
            Mappings = new Dictionary<Type, Type>();
        }
        public void Register<TViewModel, TView>()
            where TViewModel : IDialogRequestClose
            where TView : IDialog
        {
            if (Mappings.ContainsKey(typeof(TViewModel)))
            {
                throw new ArgumentException($"Type {typeof(TViewModel)} is already mapped to type {typeof(TView)} ");
            }

            Mappings.Add(typeof(TViewModel), typeof(TView));
        }

        public void ShowDialog<TViewModel>(TViewModel viewModel) where TViewModel : IDialogRequestClose
        {
            Type viewType = Mappings[typeof(TViewModel)];

            IDialog dialog = (IDialog)Activator.CreateInstance(viewType);

            EventHandler<DialogCloseRequestedEventArgs> handler = null;

            handler = (sender, e) =>
            {
                viewModel.CloseRequested -= handler;

                if (e.DialogResult.HasValue)
                {
                    dialog.DialogResult = e.DialogResult;
                }
                else
                {
                    dialog.Close();
                }
            };

            viewModel.CloseRequested += handler;

            dialog.DataContext = viewModel;
            dialog.Owner = _owner;

            dialog.ShowDialog();
            return;
        }
    }
}
