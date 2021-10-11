﻿using MVVMToolkit.DialogServices;
using MVVMToolkit.DialogServices.ViewModels;
using Ninject.Modules;

namespace MVVMToolkit.Ioc
{
    /// <summary>
    /// Module for dependency injection of main view model
    /// Binds main view model to service
    /// </summary>
    class Module : NinjectModule
    {
        public override void Load()
        {
            Bind<IDialogService>().To<DialogService>().InSingletonScope();
            Bind<MainViewModel>().ToSelf();
        }
    }
}
