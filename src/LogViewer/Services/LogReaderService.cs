// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogReaderService.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
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
    using MethodTimer;
    using Models;
    using Orc.FileSystem;

    public class LogReaderService : ILogReaderService
    {
        #region Fields
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private static readonly Regex LogRecordPattern = new Regex(@"^(\d{4}-\d{2}-\d{2}\s)?\d{2}\:\d{2}\:\d{2}\:\d+\s\=\>\s\[[a-zA-Z]+\]\s\[[a-zA-Z\d\.\`]+\].+", RegexOptions.Compiled);
        private static readonly Regex DateTimePattern = new Regex(@"^(\d{4}-\d{2}-\d{2}\s)?\d{2}\:\d{2}\:\d{2}\:\d+", RegexOptions.Compiled);
        private static readonly Regex ThreadIdPattern = new Regex(@"^\[[0-9\.]+\]", RegexOptions.Compiled);
        private static readonly Regex LogEventPattern = new Regex(@"^\[[a-zA-Z]+\]", RegexOptions.Compiled);
        private static readonly Regex TargetTypePattern = new Regex(@"^\[[a-zA-Z\.\`\d]+\]", RegexOptions.Compiled);

        private readonly IFileService _fileService;

        #endregion

        #region Constructors

        public LogReaderService(IFileService fileService)
        {
            Argument.IsNotNull(() => fileService);

            _fileService = fileService;
        }

        #endregion

        #region Methods

        #region ILogReaderService Members

        public IEnumerable<LogRecord> LoadRecordsFromFile(FileNode fileNode)
        {
            Argument.IsNotNull(() => fileNode);

            FileStream stream;

            Log.Debug("Loading records file file '{0}'", fileNode);

            try
            {
                stream = _fileService.Open(fileNode.FileInfo.FullName, FileMode.Open, FileAccess.Read);
            }
            catch (IOException ex)
            {
                Log.Warning(ex, "Failed to load records from file '{0}'", fileNode);

                yield break;
            }

            int counter = 0;
            using (stream)
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

                            record = new LogRecord
                            {
                                Position = counter++,
                                FileNode = fileNode,
                                DateTime = ExtractDateTime(ref line)
                            };

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

            Log.Info("Read '{0}' records from file '{1}'", counter, fileNode);
        }
        #endregion

        private DateTime ExtractDateTime(ref string line)
        {
            var dateTimeString = DateTimePattern.Match(line).Value;
            line = line.Substring(dateTimeString.Length + " => ".Length).TrimStart();
            return DateTime.ParseExact(dateTimeString, new[] { "HH:mm:ss:fff", "yyyy-MM-dd HH:mm:ss:fff" }, null, DateTimeStyles.NoCurrentDateDefault);
        }

        private LogEvent ExtractLogEventType(ref string line)
        {
            var eventTypeString = LogEventPattern.Match(line).Value;
            line = line.Substring(eventTypeString.Length).TrimStart();
            eventTypeString = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(eventTypeString.Trim('[', ']').ToLowerInvariant());
            return (LogEvent)Enum.Parse(typeof(LogEvent), eventTypeString);
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
            Argument.IsNotNull(() => line);

            if (logRecord == null)
            {
                return;
            }

            logRecord.Message += (Environment.NewLine + line);
        }
        #endregion
    }
}
