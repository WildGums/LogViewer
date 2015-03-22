// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogRecordsTableViewModel.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace LogViewer.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Reactive.Linq;
    using System.Threading.Tasks;
    using Catel;
    using Catel.Fody;
    using Catel.MVVM;
    using Models;
    using Services;

    public class LogRecordsTableViewModel : ViewModelBase
    {
        #region Fields
        private readonly IFilterService _filterService;
        private readonly IAggregateLogService _aggregateLogService;
        private IDisposable _applyFilterListener;
        private ObservableCollection<NavigationNode> _prevSelectedItems;
        #endregion

        #region Constructors
        public LogRecordsTableViewModel(IFilterService filterService, ICommandManager commandManager, IFileBrowserService fileBrowserService,
            IAggregateLogService aggregateLogService)
        {
            Argument.IsNotNull(() => filterService);
            Argument.IsNotNull(() => commandManager);
            Argument.IsNotNull(() => fileBrowserService);
            Argument.IsNotNull(() => aggregateLogService);

            _filterService = filterService;
            _aggregateLogService = aggregateLogService;
            FileBrowser = fileBrowserService.FileBrowserModel;
            Filter = filterService.Filter;
            AggregateLog = aggregateLogService.AggregateLog;

            ResetSearchTemplate = new Command(OnResetSearchTemplateExecute);

            commandManager.RegisterCommand(Commands.Filter.ResetSearchTemplate, ResetSearchTemplate, this);
        }
        #endregion

        #region Properties
        public Command ResetSearchTemplate { get; private set; }

        [Model]
        [Expose("SelectedItems")]
        public FileBrowserModel FileBrowser { get; set; }

        [Model]
        [Expose("Records")]
        public Log AggregateLog { get; private set; }

        [Model]
        [Expose("IsUseDateRange")]
        [Expose("StartDate")]
        [Expose("EndDate")]
        public Filter Filter { get; set; }

        [Model]
        [Expose("RegularExpression")]
        [ViewModelToModel("Filter")]
        public SearchTemplate SearchTemplate { get; set; }
        #endregion

        #region Methods
        public void OnSelectedItemsChanged()
        {
            if (_prevSelectedItems != null)
            {
                _prevSelectedItems.CollectionChanged -= OnSelectedItemsCollectionChanged;
            }

            if (FileBrowser.SelectedItems != null)
            {
                FileBrowser.SelectedItems.CollectionChanged += OnSelectedItemsCollectionChanged;
            }

            _prevSelectedItems = FileBrowser.SelectedItems;
        }

        private async void OnSelectedItemsCollectionChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            await ApplyFilter();
        }

        public void OnEndDateChanged()
        {
            _filterService.ApplyFilesFilter(FileBrowser);
        }

        public void OnStartDateChanged()
        {
            _filterService.ApplyFilesFilter(FileBrowser);
        }

        public void OnIsUseDateRangeChanged()
        {
            _filterService.ApplyFilesFilter(FileBrowser);
        }

        private async void OnSearchTemplateIsDirtyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if (Filter.UseTextSearch)
            {
                await ApplyFilter(SearchTemplate);
            }
        }

        private async void OnFilterIsDirtyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            await ApplyFilter(Filter);
        }

        private void OnResetSearchTemplateExecute()
        {
            SearchTemplate.TemplateString = string.Empty;
        }

        public async void OnSelectedItemChanged()
        {
            await ApplyFilter();
        }

        private async Task ApplyFilter(SimplyClearableModel clearableModel = null)
        {
            await Task.Factory.StartNew(() =>
            {
                if (clearableModel != null && !clearableModel.IsDirty)
                {
                    return;
                }

                _filterService.ApplyLogRecordsFilter(FileBrowser);

                if (clearableModel != null)
                {
                    clearableModel.MarkClean();
                }
            });
        }

        protected override async Task Initialize()
        {
            Filter.PropertyChanged += OnFilterIsDirtyChanged;

            var observable = Observable.FromEventPattern<PropertyChangedEventHandler, PropertyChangedEventArgs>(
                h => SearchTemplate.PropertyChanged += h,
                h => SearchTemplate.PropertyChanged -= h);

            _applyFilterListener = observable
                .Delay(TimeSpan.FromMilliseconds(500))
                .Throttle(TimeSpan.FromMilliseconds(500))
                .ObserveOnDispatcher()
                .Subscribe(async e =>
                {
                    if (Filter.UseTextSearch)
                    {
                        await ApplyFilter(SearchTemplate);
                    }
                });

            await base.Initialize();
        }

        protected override async Task Close()
        {
            Filter.PropertyChanged -= OnFilterIsDirtyChanged;

            _applyFilterListener.Dispose();

            await base.Close();
        }
        #endregion
    }
}