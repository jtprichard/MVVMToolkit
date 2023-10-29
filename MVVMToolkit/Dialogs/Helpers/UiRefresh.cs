﻿using System;
using System.Windows;
using System.Windows.Threading;

namespace PB.MVVMToolkit.Dialogs.Helpers
{
    public static class UiRefresh
    {

        private static Action EmptyDelegate = delegate () { };


        public static void Refresh(this UIElement uiElement)
        {
            uiElement.Dispatcher.Invoke(DispatcherPriority.Render, EmptyDelegate);
        }
    }
}