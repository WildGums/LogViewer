// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FilterExtensions.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace LogViewer.Extensions
{
    using System.Text.RegularExpressions;

    using Catel;
    using Catel.Logging;

    using LogViewer.Models;

    public static class FilterExtensions
    {
        #region Methods
        public static bool IsAcceptableTo(this Filter filter, LogEvent logEvent)
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

        public static bool IsAcceptableTo(this Filter filter, string message)
        {
            Argument.IsNotNull(() => message);

            if (!filter.UseTextSearch || filter.SearchTemplate.RegularExpression == null)
            {
                return true;
            }

            return Regex.IsMatch(message, filter.SearchTemplate.RegularExpression);
        }

        public static bool IsAcceptableTo(this Filter filter, LogFile logFile)
        {
            Argument.IsNotNull(() => logFile);

            return !filter.UseDateRange || !logFile.IsUnifyNamed || (logFile.DateTime.Date <= filter.EndDate.Date && logFile.DateTime.Date >= filter.StartDate.Date);
        }
        #endregion
    }
}