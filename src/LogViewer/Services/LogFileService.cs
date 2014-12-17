namespace LogViewer.Services
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using Models;

    public class LogFileService : ILogFileService
    {
        private readonly ILogRecordService _logRecordService;

        public LogFileService(ILogRecordService logRecordService)
        {
            _logRecordService = logRecordService;
        }

        public IEnumerable<LogFile> GetLogFIles(string filesFolder)
        {
            return Directory.GetFiles(filesFolder, "*.log", SearchOption.TopDirectoryOnly).Select(InitializeLogFIle).ToArray();
        }

        public LogFile InitializeLogFIle(string fileName)
        {
            var logFile = new LogFile();
            logFile.Info = new FileInfo(fileName);
            logFile.Name = logFile.Info.Name;
            logFile.IsUnifyNamed = Regex.IsMatch(logFile.Info.Name, @"^[a-zA-Z\.]+_(\d{4}-\d{2}-\d{2})_\d{6}_\d+\.log$");
            if (!logFile.IsUnifyNamed)
            {
                logFile.Name = logFile.Info.Name;                
            }
            else
            {
                logFile.Name = logFile.Info.Name;
                var dateTimeString = Regex.Match(logFile.Info.Name, @"(\d{4}-\d{2}-\d{2})").Value;
                logFile.DateTime = DateTime.ParseExact(dateTimeString, "yyyy-MM-dd", null, DateTimeStyles.None);                
            }
            

            logFile.LogRecords = new ObservableCollection<LogRecord>(_logRecordService.LoadRecordsFromFile(logFile));

            return logFile;
        }

        private string ExtractPrefix(ref string line)
        {
            var prefix = Regex.Match(line, @"^[a-zA-Z\.]+");
            line = line.Substring(prefix.Length).TrimStart('_');
            return prefix.Value;
        }
    }
}