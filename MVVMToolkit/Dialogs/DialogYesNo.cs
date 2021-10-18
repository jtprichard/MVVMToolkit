﻿using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using PB.MVVMToolkit.DialogServices;
using PB.MVVMToolkit.ViewModel;

namespace PB.MVVMToolkit.Dialogs
{
    /// <summary>
    /// A dialog class to ask a question and return a DialogResult of Yes or No
    /// </summary>
    public class DialogYesNo : DialogViewModelBase
    {
        #region Properties

        /// <summary>
        /// A static reference to the viewmodel
        /// </summary>
        internal static DialogYesNo Instance { get; set; }
        /// <summary>
        /// Dialog message as string
        /// </summary>
        public string Message { get; private set; }
        /// <summary>
        /// Dialog caption as string
        /// </summary>
        public string Caption { get; private set; }

        /// <summary>
        /// Dialog Result
        /// Dialog will return Yes or No
        /// </summary>
        private DialogResult Result { get; set; }

        #endregion

        #region Commands

        private ICommand yesCommand = null;
        /// <summary>
        /// Yes Click Command
        /// </summary>
        public ICommand YesCommand
        {
            get { return yesCommand; }
            set { yesCommand = value; }
        }

        private ICommand noCommand = null;
        /// <summary>
        /// No Click Command
        /// </summary>
        public ICommand NoCommand
        {
            get { return noCommand; }
            set { noCommand = value; }
        }

        #endregion

        #region Constructors
        /// <summary>
        /// A dialog asks a question and return a DialogResult of Yes or No
        /// </summary>
        /// <param name="message">Dialog message as string</param>
        /// <param name="caption">Dialog window caption as string</param>
        private DialogYesNo(string message, string caption, DialogImage image = DialogImage.None)
        {
            Message = message;
            Caption = caption;
            Image = image;

            this.yesCommand = new RelayCommand(OnYesClicked);
            this.noCommand = new RelayCommand(OnNoClicked);
            Instance = this;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// A dialog asks a question and return a DialogResult of Yes or No
        /// </summary>
        /// <param name="message">Dialog message as string</param>
        /// <param name="caption">Dialog window caption as string</param>
        /// <returns></returns>
        public static DialogResult Show(string message, string caption, DialogImage image = DialogImage.None)
        {
            var vm = new DialogYesNo(message, caption, image);
            vm.Show();
            return vm.Result;
        }

        #endregion

        #region Private Methods
        /// <summary>
        /// Show dialog
        /// </summary>
        private void Show()
        {
            var view = new DialogYesNoView();
            view.ShowDialog();
        }
        /// <summary>
        /// Yes clicked command event
        /// </summary>
        /// <param name="parameter"></param>
        private void OnYesClicked(object parameter)
        {
            Result = DialogResult.Yes;
            CloseDialog(parameter as Window);
        }
        /// <summary>
        /// No clicked command event
        /// </summary>
        /// <param name="parameter"></param>
        private void OnNoClicked(object parameter)
        {
            Result = DialogResult.No;
            CloseDialog(parameter as Window);
        }

        #endregion

    }
}
