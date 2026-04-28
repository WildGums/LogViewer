namespace LogViewer.Models
{
    using System;
    using System.Collections.Generic;

    using Catel.Data;
    using Catel.Logging;
    using Microsoft.Extensions.Logging;

    public class LogRecord : ModelBase
    {
        private static readonly Dictionary<LogLevel, string> LogLevelCache = new Dictionary<LogLevel, string>();

        static LogRecord()
        {
            foreach (LogLevel value in Enum.GetValues(typeof(LogLevel)))
            {
                LogLevelCache.Add(value, value.ToString().ToUpper());
            }
        }

        public FileNode FileNode { get; set; }

        /// <summary>
        /// Position of the record in the file. Incremental id for internal use.
        /// </summary>
        public int Position { get; set; }

        public DateTime DateTime { get; set; }

        public LogLevel LogLevel { get; set; }

        public string TargetTypeName { get; set; }

        public string ThreadId { get; set; }

        public string Message { get; set; }

        public bool IsSelected { get; set; }

        public override string ToString()
        {
            return $"{DateTime:HH:mm:ss:ms} => [{LogLevelCache[LogLevel]}] [{TargetTypeName}] [{ThreadId}] {Message}";
        }
    }
}
