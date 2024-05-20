using System;
using System.ComponentModel;

namespace PB.MVVMToolkit.Dialogs.Data
{
    public class DialogInput : INotifyPropertyChanged
    {

        #region Properties
        /// <summary>
        /// Input Parameter Description
        /// </summary>
        public string Description { get; set; }

        private string _caption;
        /// <summary>
        /// Dialog caption as string
        /// </summary>
        public string Caption 
        {
            get { return _caption; }
            set {_caption = value; OnPropertyChanged(nameof(Caption));
        }
    }

        private string _answer;
        /// <summary>
        /// Default answer to show in the dialog answer textbox
        /// </summary>
        public string Answer
        {
            get { return _answer; }
            set { _answer = value; OnPropertyChanged(nameof(Answer)); }

        }

        private int _id;
        /// <summary>
        /// The Id of the item (optional)
        /// </summary>
        public int Id
        {
            get { return _id; }
            set { _id = value; OnPropertyChanged(nameof(Id)); }
        }

        /// <summary>
        /// Sets whether the input item can be a duplicate
        /// </summary>
        public bool DuplicateAllowed { get; set; }

        /// <summary>
        /// Sets whether the input response is required
        /// </summary>
        public bool Required { get; set; }

        /// <summary>
        /// Notes whether input is enabled for editing
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// The Type for the input to return
        /// </summary>
        public Type InputType { get; set; }

        public DialogInput(){}

        public DialogInput(string description, string caption, string defaultAnswer = "")
        {
            Description = description;
            Caption = caption;
            Answer = defaultAnswer;
            DuplicateAllowed = true;
            Required = false;
            IsEnabled = true;
            InputType = typeof(string);
        }

        #endregion

        #region Property Changed

        /// <summary>
        /// Occurs when a property value changes
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };

        /// <summary>
        /// Property Changed Method
        /// </summary>
        /// <param name="name"></param>
        public void OnPropertyChanged(string name)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        #endregion


    }
}
