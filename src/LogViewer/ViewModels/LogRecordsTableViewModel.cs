// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogRecordsTableViewModel.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace LogViewer.ViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Threading.Tasks;
    using Catel.Collections;
    using Catel.Fody;
    using Catel.MVVM;
    using Models;
    using Models.Base;
    using Services;

    public class LogRecordsTableViewModel : ViewModelBase
    {
        private readonly IFilterService _filterService;

        public LogRecordsTableViewModel(LogViewerModel logViewerModel, IFilterService filterService, ICommandManager commandManager)
        {
            _filterService = filterService;
            LogViewer = logViewerModel;
            LogRecords = new ObservableCollection<LogRecord>();

            ResetSearchTemplate = new Command(OnResetSearchTemplateExecute);

            commandManager.RegisterCommand("Filter.ResetSearchTemplate", ResetSearchTemplate, this);

            Filter.PropertyChanged += Filter_PropertyChanged;
            SearchTemplate.PropertyChanged += SearchTemplate_PropertyChanged;
        }

        public Command ResetSearchTemplate { get; private set; }

        [Model]
        public LogViewerModel LogViewer { get; set; }

        [ViewModelToModel("LogViewer")]
        public NavigationNode SelectedItem { get; set; }

        [Model]
        [ViewModelToModel("LogViewer")]
        public Filter Filter { get; set; }

        public ObservableCollection<LogRecord> LogRecords { get; set; }

        [Model]
        [Expose("RegularExpression")]
        [ViewModelToModel("Filter")]
        public SearchTemplate SearchTemplate { get; set; }

        private void SearchTemplate_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!SearchTemplate.IsDirty)
            {
                return;
            }

            SearchTemplate.ClearDirtyFlag();
            ApplyFilter();
        }

        private void Filter_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!Filter.IsDirty)
            {
                return;
            }

            Filter.ClearDirtyFlag();
            ApplyFilter();
        }

        private void OnResetSearchTemplateExecute()
        {
            SearchTemplate.TemplateString = string.Empty;
        }

        public void OnSelectedItemChanged()
        {
            ApplyFilter();
        }

        private async Task ApplyFilter()
        {
            var logFiles = GetLogFiles(SelectedItem);
            var filteredFiles = _filterService.FilterFIles(LogViewer.Filter, logFiles);
            LogRecords.Clear();
            LogRecords.AddRange(new ObservableCollection<LogRecord>(_filterService.FilterRecords(LogViewer.Filter, filteredFiles.SelectMany(file => file.LogRecords))));
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