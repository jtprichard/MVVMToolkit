using System;
using System.Windows;

namespace PB.MVVMToolkit.ProgressForms
{
    /// <summary>
    /// Interaction logic for DialogInputOkCancelView.xaml
    /// </summary>
    internal partial class ProgressFormView : Window
    {
        internal ProgressFormView()
        {
            InitializeComponent();
            DataContext = ProgressForm.Instance;
        }

    }
}
