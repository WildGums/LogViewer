namespace LogViewer.Services
{
    using System.Collections.Generic;
    using Models;

    public interface ILogRecordService
    {
        IEnumerable<LogRecord> LoadRecordsFromFile(string filePath);
    }
}