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
        internal DialogMultiInputOkCancelView()
        {
            InitializeComponent();
            DataContext = DialogMultiInputOkCancel.Instance;
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            //VirtualizingStackPanel.SetIsVirtualizing(lstListBox, false);
            //ListViewItem item = lstListBox.ItemContainerGenerator.ContainerFromIndex(0) as ListViewItem;
            ////item.Focus();
            //Keyboard.Focus(item);

            //lstListBox.SelectAll();
            //lstListBox.Focus();
            
            //ListBoxItem item = (ListBoxItem)(this.lstListView.ItemContainerGenerator.ContainerFromItem(c));



            //if (item.IsLoaded)

            //    FocusItem(item);

            //else

            //    item.Loaded += new RoutedEventHandler(item_Loaded);

        }

        private void item_Loaded(object sender, RoutedEventArgs e)

        {

            ListViewItem item = (ListViewItem)e.Source;

            FocusItem(item);

            item.Loaded -= new RoutedEventHandler(item_Loaded);

        }

        private void LstListView_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListView lv = sender as ListView;
            //var lvItem = (ListViewItem)lv.SelectedItem;
            //var item = lv.ItemContainerGenerator.ContainerFromItem(lv.SelectedItem);
            //var temp = lv.ItemContainerStyle.TargetType;
            return;
            //FocusItem(item);

            //var container = lv.ItemContainerStyle.TargetType;
            //var cont2 = lv.ItemContainerGenerator.ContainerFromIndex(0);
            //ListViewItem lvi = (ListViewItem)lv.ItemContainerGenerator.ContainerFromItem(lv.SelectedItem);
            //TextBox textBox = GetVisualChild<TextBox>(lvi);
            //if (textBox != null)
            //    textBox.Focus();
        }

        private T GetVisualChild<T>(Visual parent) where T : Visual
        {
            T child = default(T);
            int numVisuals = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < numVisuals; i++)
            {
                Visual v = (Visual)VisualTreeHelper.GetChild(parent, i);
                child = v as T;
                if (child == null)
                {
                    child = GetVisualChild<T>(v);
                }
                if (child != null)
                {
                    break;
                }
            }
            return child;
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {

            try
                {
                    if (e.Key == Key.A)
                    {

                    // Locate current item.
                    var currentLstView = ((sender as TextBox).TemplatedParent as ContentPresenter).Content;
                    int currentIndex = lstListView.Items.IndexOf(currentLstView);

                    //Locate the next item
                    int next = currentIndex + 1;

                    if (next > lstListView.Items.Count - 1)
                    {
                        BtnOk.Focus();
                        return;

                    }

                    // Focus the item.
                    //(lstListView.Items[next] as TextBox).Focus();

                    var nextLstView = lstListView.Items[next];

                    ListViewItem myListViewItem =
                        (ListViewItem)lstListView.ItemContainerGenerator.ContainerFromItem(nextLstView);


                    // Getting the currently selected ListBoxItem
                    // Note that the ListBox must have
                    // IsSynchronizedWithCurrentItem set to True for this to work
                    //ListViewItem myListViewItem =
                    //    (ListViewItem)lstListView.ItemContainerGenerator.ContainerFromItem(currentLstView);



                    // Getting the ContentPresenter of myListBoxItem
                    ContentPresenter myContentPresenter = FindVisualChild<ContentPresenter>(myListViewItem);

                    // Finding textBlock from the DataTemplate that is set on that ContentPresenter
                    DataTemplate myDataTemplate = myContentPresenter.ContentTemplate;
                    TextBox myTextBox = (TextBox)myDataTemplate.FindName("txtTextBox", myContentPresenter);
                    var success = myTextBox.Focus();


                    // Do something to the DataTemplate-generated TextBlock
                    //MessageBox.Show("The text of the TextBlock of the selected list item: "
                    //                + myTextBox.Text);





                    //// Locate current item.
                    //int current = lstListView.Items.IndexOf(sender);

                    //var parent = ((sender as TextBox).TemplatedParent) as ContentPresenter;
                    //var supparent = parent.Content;
                    //var supparentlst = supparent as ListViewItem;

                    //var items = lstListView.Items;

                    }
            }
            catch (Exception ex)
            {
            }
        }


        private void FocusItem(ListViewItem item)

        {

            ContentPresenter contentPresenter = FindVisualChild<ContentPresenter>(item);

            DataTemplate dataTemplate = (DataTemplate)this.Resources["lstTemplate"];

            TextBox nameTextBox = (TextBox)dataTemplate.FindName("txtTextBox", contentPresenter);

            nameTextBox.Focus();

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
    }

}

