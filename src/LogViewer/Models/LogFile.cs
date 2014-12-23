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

    using LogViewer.Models.Base;

    public class LogFile : NavigationNode
    {
        #region Fields
        private readonly bool _allowMutiselection = true;
        #endregion

        #region Properties
        public FileInfo Info { get; set; }

        public bool IsUnifyNamed { get; set; }

        public DateTime DateTime { get; set; }

        public ObservableCollection<LogRecord> LogRecords { get; set; }

        public bool? IsExpanded { get; set; }

        public override bool AllowMultiselection
        {
            get
            {
                return _allowMutiselection;
            }
        }
        #endregion
    }
}