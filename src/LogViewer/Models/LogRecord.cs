// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogRecord.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace LogViewer.Models
{
    using System;
    using Catel.Data;

    public class LogRecord : ModelBase
    {
        public DateTime DateTime { get; set; }
        public LogRecordType LogRecordType { get; set; }
        public string AssemblyName { get; set; }
        public string Text { get; set; }
    }
}