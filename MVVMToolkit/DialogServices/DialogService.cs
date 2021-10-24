using System;
using System.Collections.Generic;
using System.Windows.Controls;
using PB.MVVMToolkit.Dialogs;

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
        public static DialogService Instance { get; private set; }

        public DialogService(Page owner)
        {
            this._owner = owner;
            Mappings = new Dictionary<Type, Type>();
            Instance = this;
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
            try
            {
                Type viewType = Mappings[typeof(TViewModel)];

                IDialog dialog = (IDialog) Activator.CreateInstance(viewType);

                void handler(object sender, DialogCloseRequestedEventArgs e)
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
                }

                viewModel.CloseRequested += handler;

                dialog.DataContext = viewModel;
                dialog.Owner = _owner;

                dialog.ShowDialog();
            }
            catch (KeyNotFoundException ex)
            {
                string message = "Dialog Error.  Report error to application provider.\n\nError: \n" + ex.ToString();
                DialogOk.Show(message, "Dialog Error", DialogImage.Error);
            }

            return;
        }
    }
}
