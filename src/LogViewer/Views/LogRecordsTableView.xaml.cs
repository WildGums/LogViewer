// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogRecordsTableView.xaml.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2014 Wild Gums. All rights reserved.
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
        }

        private void InitDataGrid()
        {
            Grid.GroupStyle.Add(_defaultGroupStyle);
        }
    }
}