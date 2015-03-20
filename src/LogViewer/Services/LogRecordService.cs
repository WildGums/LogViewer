// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogRecordService.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace LogViewer.Services
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Text.RegularExpressions;
    using Catel;
    using Catel.Logging;
    using Models;

    public class LogRecordService : ILogRecordService
    {
        #region Fields
        private static readonly Regex LogRecordPattern = new Regex(@"^(\d{4}-\d{2}-\d{2}\s)?\d{2}\:\d{2}\:\d{2}\:\d+\s\=\>\s\[[a-zA-Z]+\]\s\[[a-zA-Z\d\.\`]+\].+", RegexOptions.Compiled);
        private static readonly Regex DateTimePattern = new Regex(@"^(\d{4}-\d{2}-\d{2}\s)?\d{2}\:\d{2}\:\d{2}\:\d+", RegexOptions.Compiled);
        private static readonly Regex ThreadIdPattern = new Regex(@"^\[[0-9\.]+\]", RegexOptions.Compiled);
        private static readonly Regex LogEventPattern = new Regex(@"^\[[a-zA-Z]+\]", RegexOptions.Compiled);
        private static readonly Regex TargetTypePattern = new Regex(@"^\[[a-zA-Z\.\`\d]+\]", RegexOptions.Compiled);
        #endregion

        #region Methods

        #region ILogRecordService Members
        public IEnumerable<LogRecord> LoadRecordsFromFile(FileNode fileNode)
        {
            Argument.IsNotNull(() => fileNode);
            int counter = 0;
            using (var stream = new FileStream(fileNode.FileInfo.FullName, FileMode.Open))
            {
                using (var reader = new StreamReader(stream))
                {
                    string line;
                    LogRecord record = null;

                    while ((line = reader.ReadLine()) != null)
                    {
                        if (LogRecordPattern.IsMatch(line))
                        {
                            if (record != null)
                            {
                                yield return record;
                            }

                            record = new LogRecord() {Position = counter++};
                            record.FileNode = fileNode;
                            record.DateTime = ExtractDateTime(ref line);

                            if (fileNode.IsUnifyNamed && record.DateTime.Date == DateTime.MinValue.Date)
                            {
                                record.DateTime = fileNode.DateTime.Date + record.DateTime.TimeOfDay;
                            }

                            record.LogEvent = ExtractLogEventType(ref line);
                            record.TargetTypeName = ExtractTargetTypeName(ref line);
                            record.ThreadId = ExtractThreadId(ref line);
                            record.Message = line;
                        }
                        else
                        {
                            AppendMessageLine(record, line);
                        }
                    }

                    if (record != null)
                    {
                        yield return record;
                    }
                }
            }
        }
        #endregion

        private DateTime ExtractDateTime(ref string line)
        {
            var dateTimeString = DateTimePattern.Match(line).Value;
            line = line.Substring(dateTimeString.Length + " => ".Length).TrimStart();
            return DateTime.ParseExact(dateTimeString, new[] {"hh:mm:ss:fff", "yyyy-MM-dd hh:mm:ss:fff"}, null, DateTimeStyles.NoCurrentDateDefault);
        }

        private LogEvent ExtractLogEventType(ref string line)
        {
            var eventTypeString = LogEventPattern.Match(line).Value;
            line = line.Substring(eventTypeString.Length).TrimStart();
            eventTypeString = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(eventTypeString.Trim('[', ']').ToLowerInvariant());
            return (LogEvent) Enum.Parse(typeof (LogEvent), eventTypeString);
        }

        private string ExtractTargetTypeName(ref string line)
        {
            var targetTypeName = TargetTypePattern.Match(line).Value;
            line = line.Substring(targetTypeName.Length).TrimStart();
            return targetTypeName.Trim('[', ']');
        }

        private string ExtractThreadId(ref string line)
        {
            var threadId = ThreadIdPattern.Match(line).Value;
            line = line.Substring(threadId.Length).TrimStart();
            return threadId.Trim('[', ']');
        }

        private void AppendMessageLine(LogRecord logRecord, string line)
        {
            Argument.IsNotNull(() => logRecord);
            Argument.IsNotNull(() => line);

            logRecord.Message += (Environment.NewLine + line);
        }
        #endregion
    }
}