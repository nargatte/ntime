using System.Collections;
using System.Windows;
using System.Windows.Controls;

namespace MvvmHelper
{
    public class MultipleChoiceDataGrid : DataGrid
    {
        public MultipleChoiceDataGrid()
        {
            this.SelectionChanged += MultipleChoiceDataGrid_SelectionChanged;
        }

        private void MultipleChoiceDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.SelectedItemsList = this.SelectedItems;
        }

        public IList SelectedItemsList
        {
            get { return (IList)GetValue(SelectedItemsListProperty); }
            set { SetValue(SelectedItemsListProperty, value); }
        }

        public static readonly DependencyProperty SelectedItemsListProperty =
            DependencyProperty.Register("SelectedItemsList", typeof(IList), typeof(MultipleChoiceDataGrid), new PropertyMetadata(null));
    }
}
