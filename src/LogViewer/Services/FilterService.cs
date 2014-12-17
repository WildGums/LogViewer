namespace LogViewer.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using Catel;
    using Models;

    class FilterService : IFilterService
    {
        public IEnumerable<LogRecord> FilterRecords(Filter filter, IEnumerable<LogRecord> logRecords)
        {
            return logRecords;
        }

        public IEnumerable<LogFile> FilterFIles(Filter filter, IEnumerable<LogFile> logFiles)
        {
            Argument.IsNotNull(() => logFiles);
            Argument.IsNotNull(() => filter);

            return logFiles.Where(file => !file.IsUnifyNamed || (file.DateTime.Date <= filter.EndDate.Date && file.DateTime.Date >= filter.StartDate.Date));
        }
    }
}