// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogViewerModel.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace LogViewer.Models
{
    using System.Collections.ObjectModel;

    using Catel.Data;

    using LogViewer.Models.Base;

    public class LogViewerModel : ModelBase
    {
        #region Constructors
        public LogViewerModel()
        {
            Companies = new ObservableCollection<Company>();
            Filter = new Filter();
            SelectedItems = new ObservableCollection<NavigationNode>();
            LogRecords = new ObservableCollection<LogRecord>();
        }
        #endregion

        #region Properties
        public Filter Filter { get; set; }

        public ObservableCollection<Company> Companies { get; set; }

        public ObservableCollection<NavigationNode> SelectedItems { get; set; }

        public ObservableCollection<LogRecord> LogRecords { get; set; }
        #endregion
    }
}