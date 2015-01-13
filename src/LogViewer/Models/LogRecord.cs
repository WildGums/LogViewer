// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogRecord.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2014 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace LogViewer.Models
{
    using System;

    using Catel.Data;
    using Catel.Logging;

    public class LogRecord : ModelBase
    {
        #region Properties
        public LogFile LogFile { get; set; }

        public DateTime DateTime { get; set; }

        public LogEvent LogEvent { get; set; }

        public string TargetTypeName { get; set; }

        public string Message { get; set; }
        #endregion
    }
}