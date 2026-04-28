namespace LogViewer.Views
{
    using System.Windows.Controls;
    
    public partial class LogRecordsTableView
    {
        private GroupStyle? _defaultGroupStyle;

        partial void OnInitializedComponent()
        {
            _defaultGroupStyle = (GroupStyle) FindResource("groupStyle");

            InitDataGrid();

            Loaded += LogRecordsTableView_Loaded;
        }

        private void LogRecordsTableView_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            //throw new System.NotImplementedException();
        }

        private void InitDataGrid()
        {
            if (_defaultGroupStyle is not null)
            {
                Grid.GroupStyle.Add(_defaultGroupStyle);
            }
        }
    }
}
