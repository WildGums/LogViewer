// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogFile.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace LogViewer.Models
{
    using System;
    using System.Collections.ObjectModel;
    using Base;
    using Catel.Data;
    using YAXLib;

    public class LogFile : TreeNode
    {
        public string FIleName { get; set; }
        public DateTime DateTime { get; set; }
        public int ProcessId { get; set; }
        public ReadOnlyObservableCollection<LogRecord> LogRecords { get; set; } 
        public ReadOnlyObservableCollection<LogRecord> FilteredLogRecords { get; set; } 
    }
}