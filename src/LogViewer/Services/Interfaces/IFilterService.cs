namespace LogViewer.Services
{
    using System.Collections.Generic;
    using Models;
    using Models.Base;

    public interface IFilterService
    {
        IEnumerable<LogRecord> FilterRecords(Filter filter, NavigationNode node);
        IEnumerable<LogRecord> FilterRecords(Filter filter, IEnumerable<LogRecord> logRecords);
        IEnumerable<LogFile> FilterFIles(Filter filter, IEnumerable<LogFile> logFiles);
        IEnumerable<LogFile> FilterFIles(Filter filter, NavigationNode node);
    }
}