// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SetOfLogRecords.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace LogViewer.Models
{
    using System;
    using System.Collections.ObjectModel;
    using Catel.Data;

    public class SetOfLogRecords : ModelBase
    {
        public string ProcessAssemblyName { get; set; }
        public DateTime DateTime { get; set; }
        public int ProcessId { get; set; }
        public ReadOnlyObservableCollection<LogRecord> LogRecords { get; set; } 
    }
}