namespace LogViewer.Services
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using Factories;
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

        public LogFilesGroup CreateLogFilesGroup(string name, IEnumerable<LogFile> logFiles)
        {
            var logFilesGroup = new LogFilesGroup();
            logFilesGroup.Name = name;
            logFilesGroup.LogFiles = new ObservableCollection<LogFile>(logFiles);
            return logFilesGroup;
        }

        public LogFile InitializeLogFIle(string fileName)
        {
            var logFile = new LogFile();
            logFile.Info = new FileInfo(fileName);
            logFile.Name = logFile.Info.Name;
            logFile.HasUnifiedName = Regex.IsMatch(logFile.Info.Name, @"^[a-zA-Z\.]+_(\d{4}-\d{2}-\d{2})_\d{6}_\d+\.log$");
            if (!logFile.HasUnifiedName)
            {
                logFile.Name = logFile.Info.Name;
            }
            else
            {
                var name = logFile.Info.Name;
                logFile.Name = ExtractPrefix(ref name);
            }

            // TODO: make it lazy load
            logFile.LogRecords = new ObservableCollection<LogRecord>(_logRecordService.LoadRecordsFromFile(logFile.Info));

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