using System.Windows;
using System.Windows.Controls;
using PB.MVVMToolkit.ViewModel;

namespace PB.MVVMToolkit.DialogServices
{
    public class MainPageBase : Page
    {
        public IDialogService DialogService
        {
            get { return (IDialogService)GetValue(DialogServiceProperty); }
            set { SetValue(DialogServiceProperty, value); }
        }

        public static readonly DependencyProperty DialogServiceProperty =
            DependencyProperty.Register("DialogService", typeof(IDialogService), typeof(MainPageBase), new PropertyMetadata(null, PropertyChangedCallback));

        static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Page v = d as Page;
            if (v == null)
                return;

            BaseViewModel vm = v.DataContext as BaseViewModel;
            if (vm == null)
                return;

            IDialogService ds = e.NewValue as IDialogService;
            if (ds == null)
                return;

            vm.DialogService = ds;
        }
    }
}