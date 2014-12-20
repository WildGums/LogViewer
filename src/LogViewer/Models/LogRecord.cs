// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogRecord.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace LogViewer.Models
{
    using System;
    using Catel.Data;
    using Catel.Logging;

    public class LogRecord : ModelBase
    {
        public LogFile LogFile { get; set; }
        public DateTime DateTime { get; set; }
        public LogEvent LogEvent { get; set; }
        public string TargetTypeName { get; set; }
        public string Message { get; set; }

        public bool IsSelected { get; set; }        
    }
}