// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogRecordsTableView.xaml.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace LogViewer.Views
{
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for LogRecordsTableView.xaml
    /// </summary>
    public partial class LogRecordsTableView
    {
        private readonly GroupStyle _defaultGroupStyle;

        public LogRecordsTableView()
        {
            InitializeComponent();

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
            Grid.GroupStyle.Add(_defaultGroupStyle);
        }
         
    }
}