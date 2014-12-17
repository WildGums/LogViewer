namespace LogViewer.Views
{
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for LogRecordsTableView.xaml
    /// </summary>
    public partial class LogRecordsTableView 
    {
        private GroupStyle _defaultGroupStyle;

        public LogRecordsTableView()
        {
            InitializeComponent();
            _defaultGroupStyle = (GroupStyle)this.FindResource("groupStyle");
            InitDataGrid();
        }

        private void InitDataGrid()
        {
            this.Grid.GroupStyle.Add(_defaultGroupStyle);
        }
    }
}
