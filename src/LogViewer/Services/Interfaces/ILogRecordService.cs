namespace LogViewer.Services
{
    using System.Collections.Generic;
    using System.IO;

    using Models;

    public interface ILogRecordService
    {
        IEnumerable<LogRecord> LoadRecordsFromFile(LogFile logFile);
    }
}