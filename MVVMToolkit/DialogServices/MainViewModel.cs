using MVVMToolkit.Ioc;
using Ninject;

namespace MVVMToolkit.DialogServices
{
    /// <summary>
    /// Main View Model class used for dependency injection service used for locator
    /// Locator service only monitors main view model
    /// All other model / viewmodels must be associated in Application startup - App.xaml
    /// Locator should be placed with ViewModels and register the main viewmodel class
    /// </summary>
    public class MainViewModel
    {
        /// <summary>
        /// Register the main view model for the locator
        /// </summary>
        /// <typeparam name="T">MainViewModel Class as Generic</typeparam>
        /// <returns></returns>
        public object Register<T>()
        {
            return Container.Instance.Kernel.Get<T>();
        }
    }
}
