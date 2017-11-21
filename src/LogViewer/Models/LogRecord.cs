// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogRecord.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
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
        public FileNode FileNode { get; set; }

        /// <summary>
        /// Position of the record in the file. Incremental id for internal use.
        /// </summary>
        public int Position { get; set; }

        public DateTime DateTime { get; set; }

        public LogEvent LogEvent { get; set; }

        public string TargetTypeName { get; set; }

        public string ThreadId { get; set; }

        public string Message { get; set; }

        public bool IsSelected { get; set; }
        #endregion

        public override string ToString()
        {
            return $"{DateTime:HH:mm:ss:ms} => [{LogEvent.ToString().ToUpper()}] [{TargetTypeName}] [{ThreadId}] {Message}";
        }
    }
}