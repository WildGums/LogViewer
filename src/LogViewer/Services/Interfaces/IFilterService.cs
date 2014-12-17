namespace LogViewer.Services
{
    using System.Collections.Generic;
    using Models;

    public interface IFilterService
    {
        IEnumerable<LogRecord> FilterRecords(Filter filter, IEnumerable<LogRecord> logRecords);
        IEnumerable<LogFile> FilterFIles(Filter filter, IEnumerable<LogFile> logFiles);
    }
}