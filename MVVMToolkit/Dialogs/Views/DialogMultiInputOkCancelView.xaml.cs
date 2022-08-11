using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace PB.MVVMToolkit.Dialogs
{
    /// <summary>
    /// Interaction logic for DialogInputOkCancelView.xaml
    /// </summary>
    internal partial class DialogMultiInputOkCancelView : Window
    {
        #region Constructor

        internal DialogMultiInputOkCancelView()
        {
            InitializeComponent();
            DataContext = DialogMultiInputOkCancel.Instance;
        }

        #endregion

        #region Events

        /// <summary>
        /// Startup.  Focuses first TextBox for entry
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_ContentRendered(object sender, EventArgs e)
        {
            var lstViewItem = lstListBox.Items[0];

            ListBoxItem myListBoxItem =
                (ListBoxItem)lstListBox.ItemContainerGenerator.ContainerFromItem(lstViewItem);

            FocusTextBoxItem(myListBoxItem, "txtTextBox");
        }

        /// <summary>
        /// Keydown method for Listview Item Textbox.  This method is an alternate way of
        /// implementing the XAML properties for Listviews:
        /// Focusable="True"
        /// KeyboardNavigation.TabNavigation="Continue"
        /// Unused but here for future reference
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Tab)
                {
                    // Locate current item.
                    var myListBox = ((sender as TextBox).TemplatedParent as ContentPresenter).Content;
                    int currentIndex = lstListBox.Items.IndexOf(myListBox);

                    //Locate the next item
                    int next = currentIndex + 1;

                    if (next > lstListBox.Items.Count - 1)
                    {
                        BtnOk.Focus();
                        return;

                    }

                    // Focus the item.
                    var nextListBox = lstListBox.Items[next];

                    // Getting the currently selected ListBoxItem
                    // Note that the ListBox must have
                    // IsSynchronizedWithCurrentItem set to True for this to work

                    ListBoxItem myListBoxItem =
                        (ListBoxItem)lstListBox.ItemContainerGenerator.ContainerFromItem(nextListBox);

                    // Getting the ContentPresenter of myListBoxItem
                    ContentPresenter myContentPresenter = FindVisualChild<ContentPresenter>(myListBoxItem);

                    // Find textBlock from the DataTemplate that is set on that ContentPresenter
                    DataTemplate myDataTemplate = myContentPresenter.ContentTemplate;
                    TextBox myTextBox = (TextBox)myDataTemplate.FindName("txtTextBox", myContentPresenter);
                    var success = myTextBox.Focus();
                    //Notes keypress has been handled so it stops any further events
                    e.Handled = true;
                }
            }
            catch (Exception ex)
            {
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Set focus to a textbox at a listbox item
        /// </summary>
        /// <param name="item"></param>
        /// <param name="textBoxName"></param>
        private void FocusTextBoxItem(ListBoxItem item, string textBoxName)
        {
            ContentPresenter myContentPresenter = FindVisualChild<ContentPresenter>(item);

            // Find textBlock from the DataTemplate that is set on that ContentPresenter
            DataTemplate myDataTemplate = myContentPresenter.ContentTemplate;
            TextBox myTextBox = (TextBox)myDataTemplate.FindName(textBoxName, myContentPresenter);
            myTextBox.Focus();

        }

        private static childItem FindVisualChild<childItem>(DependencyObject obj)
            where childItem : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                if (child != null && child is childItem)
                    return (childItem)child;
                else
                {
                    childItem childOfChild = FindVisualChild<childItem>(child);
                    if (childOfChild != null)
                        return childOfChild;
                }
            }
            return null;
        }

        #endregion
    }
}

