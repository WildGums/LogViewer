// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogViewerModel.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2014 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace LogViewer.Models
{
    using System.Collections.ObjectModel;

    using Catel.Data;

    public class LogViewerModel : ModelBase
    {
        #region Constructors
        public LogViewerModel()
        {
            Filter = new Filter();

            Companies = new ObservableCollection<Company>();
            SelectedItems = new ObservableCollection<NavigationNode>();
            LogRecords = new ObservableCollection<LogRecord>();
        }
        #endregion

        #region Properties
        public Filter Filter { get; set; }

        public ObservableCollection<Company> Companies { get; private set; }

        public ObservableCollection<NavigationNode> SelectedItems { get; private set; }

        public ObservableCollection<LogRecord> LogRecords { get; private set; }
        #endregion
    }
}