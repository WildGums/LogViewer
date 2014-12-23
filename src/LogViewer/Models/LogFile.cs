// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogFile.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace LogViewer.Models
{
    using System;
    using System.Collections.ObjectModel;
    using System.IO;

    public class LogFile : NavigationNode
    {
        public LogFile()
        {
            LogRecords = new ObservableCollection<LogRecord>();
        }

        #region Properties
        public FileInfo Info { get; set; }

        public bool IsUnifyNamed { get; set; }

        public DateTime DateTime { get; set; }

        public ObservableCollection<LogRecord> LogRecords { get; private set; }

        public bool? IsExpanded { get; set; }

        public override bool AllowMultiSelection
        {
            get { return true; }
        }
        #endregion
    }
}