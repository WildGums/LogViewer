// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogRecordsTableViewModel.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace LogViewer.ViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Data;
    using Behaviors;
    using Catel;
    using Catel.Collections;
    using Catel.MVVM;
    using Extensions;
    using LogViewer.Models;
    using LogViewer.Models.Base;
    using Services;

    public class LogRecordsTableViewModel : ViewModelBase
    {
        private readonly IFilterService _filterService;

        public LogRecordsTableViewModel(LogViewerModel logViewerModel, IFilterService filterService, ICommandManager commandManager)
        {
            _filterService = filterService;
            LogViewer = logViewerModel;
            LogRecords = new ObservableCollection<LogRecord>();

            ApplyFilter = new Command(OnApplyFilterExecute);

            commandManager.RegisterCommand("Filter.ApplyFilter", ApplyFilter, this);
        }

        private void OnApplyFilterExecute()
        {
            OnSelectedItemChanged();
        }

        public Command ApplyFilter { get; private set; }

        [Model]
        public LogViewerModel LogViewer { get; set; }

        [ViewModelToModel("LogViewer")]
        public NavigationNode SelectedItem { get; set; }

        public ObservableCollection<LogRecord> LogRecords { get; set; }

        public void OnSelectedItemChanged()
        {
            var logFiles = GetLogFiles(SelectedItem);
            var filteredFiles = _filterService.FilterFIles(LogViewer.Filter, logFiles);
            LogRecords.Clear();
            LogRecords.AddRange(new ObservableCollection<LogRecord>(_filterService.FilterRecords(LogViewer.Filter,filteredFiles.SelectMany(file => file.LogRecords))));
        }

        private IEnumerable<LogFile> GetLogFiles(NavigationNode node)
        {
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
                    foreach (var logFile in product.LogFiles)
                    {
                        yield return logFile;
                    }
                }
            }
        }
    }
}