namespace LogViewer.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    using Catel;
    using Catel.Logging;

    using Models;
    using Models.Base;

    class FilterService : IFilterService
    {
        public IEnumerable<LogRecord> FilterRecords(Filter filter, IEnumerable<LogFile> files)
        {
            return FilterRecords(filter, FilterFIles(filter, files).SelectMany(file => file.LogRecords));
        }

        public IEnumerable<LogRecord> FilterRecords(Filter filter, IEnumerable<LogRecord> logRecords)
        {
            return logRecords.Where(record => AcceptFilterToLogEvent(record.LogEvent, filter) && AcceptFilterToMessageText(record.Message, filter));
        }

        public IEnumerable<LogFile> FilterFIles(Filter filter, IEnumerable<LogFile> logFiles)
        {
            Argument.IsNotNull(() => logFiles);
            Argument.IsNotNull(() => filter);

            return logFiles.Where(file => !filter.UseFilterRange || !file.IsUnifyNamed || (file.DateTime.Date <= filter.EndDate.Date && file.DateTime.Date >= filter.StartDate.Date));
        }

        public IEnumerable<LogFile> FilterFIles(Filter filter, NavigationNode node)
        {
            return FilterFIles(filter, GetLogFiles(node));
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

        private bool AcceptFilterToMessageText(string message, Filter filter)
        {
            if (!filter.UseTextSearch || filter.SearchTemplate.RegularExpression == null)
            {
                return true;
            }

            return Regex.IsMatch(message, filter.SearchTemplate.RegularExpression);
        }

        private IEnumerable<LogFile> GetLogFiles(NavigationNode node)
        {
            var logFile = node as LogFile;
            if (logFile != null)
            {
                if (logFile.IsExpanded == null)
                {
                    logFile.IsExpanded = true;
                }
                yield return logFile;
                yield break;
            }

            var stack = new Stack<NavigationNode>();
            stack.Push(node);
            while (stack.Count != 0)
            {
                var currentNode = stack.Pop();
                var product = currentNode as Product;
                if (product == null)
                {
                    foreach (var child in currentNode.Children)
                    {
                        stack.Push(child);
                    }
                }
                else
                {
                    foreach (var item in product.LogFiles)
                    {
                        yield return item;
                    }
                }
            }
        }
    }
}