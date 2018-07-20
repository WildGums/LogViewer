// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FilterExtensions.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace LogViewer
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

        public static bool IsAcceptableTo(this Filter filter, FileNode fileNode)
        {
            Argument.IsNotNull(() => fileNode);

            return !filter.IsUseDateRange || !fileNode.IsUnifyNamed || (fileNode.DateTime.Date <= filter.EndDate.Date && fileNode.DateTime.Date >= filter.StartDate.Date);
        }
        #endregion
    }
}