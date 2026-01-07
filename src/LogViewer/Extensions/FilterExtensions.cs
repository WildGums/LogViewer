namespace LogViewer
{
    using System;
    using System.Text.RegularExpressions;
    using Catel.Logging;

    using LogViewer.Models;
    using Microsoft.Extensions.Logging;

    public static class FilterExtensions
    {
        public static bool IsAcceptableTo(this Filter filter, LogLevel logLevel)
        {
            switch (logLevel)
            {
                case LogLevel.Debug:
                    return filter.ShowDebug;

                case LogLevel.Critical:
                case LogLevel.Error:
                    return filter.ShowError;

                case LogLevel.Information:
                    return filter.ShowInfo;

                case LogLevel.Warning:
                    return filter.ShowWarning;
            }

            return false;
        }

        public static bool IsAcceptableTo(this Filter filter, string message)
        {
            ArgumentNullException.ThrowIfNull(message);

            if (!filter.UseTextSearch || filter.SearchTemplate.RegularExpression is null)
            {
                return true;
            }

            return Regex.IsMatch(message, filter.SearchTemplate.RegularExpression, RegexOptions.None, TimeSpan.FromSeconds(1));
        }

        public static bool IsAcceptableTo(this Filter filter, FileNode fileNode)
        {
            ArgumentNullException.ThrowIfNull(fileNode);

            return !filter.IsUseDateRange || !fileNode.IsUnifyNamed || (fileNode.DateTime.Date <= filter.EndDate.Date && fileNode.DateTime.Date >= filter.StartDate.Date);
        }
    }
}
