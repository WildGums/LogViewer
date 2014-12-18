namespace LogViewer.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using Catel;
    using Catel.Logging;

    using Models;

    class FilterService : IFilterService
    {
        public IEnumerable<LogRecord> FilterRecords(Filter filter, IEnumerable<LogRecord> logRecords)
        {
            return logRecords.Where(record => AcceptFilterToLogEvent(record.LogEvent, filter));
        }

        public IEnumerable<LogFile> FilterFIles(Filter filter, IEnumerable<LogFile> logFiles)
        {
            Argument.IsNotNull(() => logFiles);
            Argument.IsNotNull(() => filter);

            return logFiles.Where(file => !file.IsUnifyNamed || (file.DateTime.Date <= filter.EndDate.Date && file.DateTime.Date >= filter.StartDate.Date));
        }

        private bool AcceptFilterToLogEvent(LogEvent logEvent, Filter filter)
        {
            switch (logEvent)
            {
                case LogEvent.Debug:
                    return filter.ShowDebug;
                case LogEvent.Error:
                    return filter.ShowError;
                case LogEvent.Info:
                    return filter.ShowInfo;
                case LogEvent.Warning:
                    return filter.ShowWarning;
            }

            return false;
        }
    }
}