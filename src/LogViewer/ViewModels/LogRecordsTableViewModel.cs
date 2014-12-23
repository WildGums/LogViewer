// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogRecordsTableViewModel.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace LogViewer.ViewModels
{
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Linq;
    using System.Threading.Tasks;
    using Catel;
    using Catel.Collections;
    using Catel.Fody;
    using Catel.MVVM;
    using Models;
    using Models.Base;
    using Services;

    public class LogRecordsTableViewModel : ViewModelBase
    {
        private ObservableCollection<NavigationNode> _prevSelectedItems;
        private readonly IFilterService _filterService;

        public LogRecordsTableViewModel(LogViewerModel logViewerModel, IFilterService filterService, ICommandManager commandManager)
        {
            Argument.IsNotNull(() => logViewerModel);
            Argument.IsNotNull(() => filterService);
            Argument.IsNotNull(() => commandManager);

            _filterService = filterService;
            LogViewer = logViewerModel;

            ResetSearchTemplate = new Command(OnResetSearchTemplateExecute);

            commandManager.RegisterCommand("Filter.ResetSearchTemplate", ResetSearchTemplate, this);

            Filter.PropertyChanged += OnFilterIsDirtyChanged;
            SearchTemplate.PropertyChanged += OnSearchTemplateIsDirtyChanged;
        }

        public Command ResetSearchTemplate { get; private set; }

        [Model]
        public LogViewerModel LogViewer { get; set; }

        [ViewModelToModel("LogViewer")]
        public ObservableCollection<NavigationNode> SelectedItems { get; set; }

        [ViewModelToModel("LogViewer")]
        public ObservableCollection<LogRecord> LogRecords { get; set; }

        [Model]
        [Expose("UseFilterRange")]
        [Expose("StartDate")]
        [Expose("EndDate")]
        [ViewModelToModel("LogViewer")]
        public Filter Filter { get; set; }        

        [Model]
        [Expose("RegularExpression")]
        [ViewModelToModel("Filter")]
        public SearchTemplate SearchTemplate { get; set; }

        public void OnSelectedItemsChanged()
        {
            if (_prevSelectedItems != null)
            {
                _prevSelectedItems.CollectionChanged -= OnSelectedItemsCollectionChanged;
            }

            if (SelectedItems != null)
            {
                SelectedItems.CollectionChanged += OnSelectedItemsCollectionChanged;
            }

            _prevSelectedItems = SelectedItems;
        }

        private void OnSelectedItemsCollectionChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            ApplyFilter();
        }

        public void OnEndDateChanged()
        {
            FilterFiles();
        }

        public void OnStartDateChanged()
        {
            FilterFiles();
        }

        public void OnUseFilterRangeChanged()
        {
            FilterFiles();
        }

        private void FilterFiles()
        {
            var buff = SelectedItems.OfType<LogFile>().ToArray();
            if (buff.Any())
            {
                while (SelectedItems.Any())
                {
                    SelectedItems.RemoveAt(0);
                }
                SelectedItems.AddRange(_filterService.FilterFIles(Filter, buff));
            }

            foreach (var company in LogViewer.Companies)
            {
                foreach (var product in company.Children.Cast<Product>())
                {
                    product.Children.Clear();
                    product.Children.AddRange(_filterService.FilterFIles(Filter, product.LogFiles).OrderByDescending(x => x.Name));
                }
            }
        }

        private void OnSearchTemplateIsDirtyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if (Filter.UseTextSearch)
            {
                ApplyFilter(SearchTemplate);
            }
        }

        private void OnFilterIsDirtyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            ApplyFilter(Filter);
        }

        private void OnResetSearchTemplateExecute()
        {
            SearchTemplate.TemplateString = string.Empty;
        }

        public void OnSelectedItemChanged()
        {
            ApplyFilter();
        }

        private async Task ApplyFilter(SimplyClearableModel clearableModel = null)
        {
            if (clearableModel != null && !clearableModel.IsDirty)
            {
                return;
            }

            var oldRecords = LogRecords.ToArray();
            LogRecords.ReplaceRange(_filterService.FilterRecords(LogViewer.Filter, SelectedItems.OfType<LogFile>()));

            foreach (var record in LogRecords.Except(oldRecords))
            {
                record.LogFile.IsExpanded = true;
            }

            if (clearableModel != null)
            {
                clearableModel.MarkClean();
            }
        }
    }
}