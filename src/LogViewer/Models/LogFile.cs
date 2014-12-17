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
    using Base;
    using Catel.Data;

    public class LogFile : NavigationNode
    {
        public FileInfo Info { get; set; }
        public bool IsUnifyNamed { get; set; }
        public DateTime DateTime { get; set; }
        public int ProcessId { get; set; }        
        public ObservableCollection<LogRecord> LogRecords { get; set; } 
        public ObservableCollection<LogRecord> FilteredLogRecords { get; set; } 
    }
}