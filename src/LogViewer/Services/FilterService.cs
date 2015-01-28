// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FilterService.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2014 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace LogViewer.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Catel;
    using Catel.Collections;

    using LogViewer.Models;

    internal class FilterService : IFilterService
    {
        #region IFilterService Members
        private IEnumerable<LogRecord> FilterRecords(Filter filter, IEnumerable<LogFile> logFiles)
        {
            Argument.IsNotNull(() => logFiles);
            Argument.IsNotNull(() => filter);

            if (!filter.SearchTemplate.UseFullTextSearch || string.IsNullOrEmpty(filter.SearchTemplate.TemplateString))
            {
                return FilterFiles(filter, logFiles).SelectMany(file => file.LogRecords).Where(record => filter.IsAcceptableTo(record.LogEvent) && filter.IsAcceptableTo(record.Message));
            }

            Func<LogRecord, bool> where = record => filter.IsAcceptableTo(record.LogEvent);
            return FilterFiles(filter, logFiles) // select only approriate files
                .SelectMany(file => file.Select(filter.SearchTemplate.TemplateString, where)) // select records and scores from each file
                .OrderBy(t => t.Item2) // sort by relevance
                .Select(t => t.Item1); // we don't need score anymore
        }

        private IEnumerable<LogFile> FilterFiles(Filter filter, IEnumerable<LogFile> logFiles)
        {
            Argument.IsNotNull(() => logFiles);
            Argument.IsNotNull(() => filter);

            return logFiles.Where(filter.IsAcceptableTo);
        }

        public void ApplyFilesFilter(LogViewerModel logViewer)
        {
            Argument.IsNotNull(() => logViewer);

            FilterSelectedFiles(logViewer);

            FilterAllFiles(logViewer);
        }

        public void ApplyLogRecordsFilter(LogViewerModel logViewer)
        {
            Argument.IsNotNull(() => logViewer);

            var logRecords = logViewer.LogRecords;

            var oldRecords = logRecords.ToArray();
            logRecords.ReplaceRange(FilterRecords(logViewer.Filter, logViewer.SelectedItems.OfType<LogFile>()));

            foreach (var record in logRecords.Except(oldRecords))
            {
                record.LogFile.IsExpanded = true;
            }
        }

        private void FilterSelectedFiles(LogViewerModel logViewer)
        {
            Argument.IsNotNull(() => logViewer);

            var selectedItems = logViewer.SelectedItems;

            var buff = selectedItems.OfType<LogFile>().ToArray();
            if (buff.Any())
            {
                while (selectedItems.Any())
                {
                    selectedItems.RemoveAt(0);
                }

                selectedItems.AddRange(FilterFiles(logViewer.Filter, buff));
            }
        }

        private void FilterAllFiles(LogViewerModel logViewer)
        {
            Argument.IsNotNull(() => logViewer);

            foreach (var company in logViewer.Companies)
            {
                foreach (var product in company.Children.Cast<Product>())
                {
                    var children = product.Children;

                    children.Clear();
                    children.AddRange(FilterFiles(logViewer.Filter, product.LogFiles).OrderByDescending(x => x.Name));
                }
            }
        }
        #endregion
    }
}