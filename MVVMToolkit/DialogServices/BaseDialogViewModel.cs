﻿using System;
using System.Windows;
using PB.MVVMToolkit.Dialogs;
using PB.MVVMToolkit.ViewModel;

namespace PB.MVVMToolkit.DialogServices
{
    /// <summary>
    /// Base View Model Class to be extended by dialogs
    /// </summary>
    public abstract class BaseDialogViewModel : BaseViewModel, IDialogRequestClose
    {
        #region Event Handler

        public event EventHandler<DialogCloseRequestedEventArgs> CloseRequested;

        #endregion

        #region Private Fields

        private static readonly string _imagePath = "pack://application:,,,/PB.MVVMToolkit;component/Resources/Images/";

        #endregion

        #region Properties

        /// <summary>
        /// Dialog image
        /// </summary>
        public DialogImage Image { get; set; }

        /// <summary>
        /// The image file name to use
        /// </summary>
        public string ImageFile => SelectImage(Image);

        /// <summary>
        /// Window object
        /// </summary>
        public Window Owner { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Closes dialog window
        /// </summary>
        /// <param name="view"></param>
        public void CloseDialog(Window view)
        {
            if (view != null)
                view.DialogResult = true;
        }

        /// <summary>
        /// Image selection for Dialog images
        /// </summary>
        /// <param name="image">Image enumeration</param>
        /// <returns>Selected image file as string</returns>
        protected string SelectImage(DialogImage image)
        {
            switch (image)
            {
                case DialogImage.Error:
                    return _imagePath + "dialog_error_icon.png";
                case DialogImage.Info:
                    return _imagePath + "dialog_information_icon.png";
                case DialogImage.Warning:
                    return _imagePath + "dialog_warning_icon.png";
                case DialogImage.Question:
                    return _imagePath + "dialog_question_icon.png";
                case DialogImage.Ok:
                    return _imagePath + "dialog_ok_icon.png";
                case DialogImage.None:
                    return "";
                default:
                    return "";
            }
        }

        #endregion

    }
}
