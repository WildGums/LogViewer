// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogRecordsTableView.xaml.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
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