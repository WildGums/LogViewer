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
        private readonly IFilterService _filterService;
        private readonly ILogTableService _logTableService;
#pragma warning disable IDISP006 // Implement IDisposable
        private IDisposable _applyFilterListener;
#pragma warning restore IDISP006 // Implement IDisposable
        private ObservableCollection<NavigationNode> _prevSelectedItems;

        public LogRecordsTableViewModel(IFilterService filterService, ICommandManager commandManager, IFileBrowserService fileBrowserService,
            ILogTableService logTableService)
        {
            ArgumentNullException.ThrowIfNull(filterService);
            ArgumentNullException.ThrowIfNull(commandManager);
            ArgumentNullException.ThrowIfNull(fileBrowserService);
            ArgumentNullException.ThrowIfNull(logTableService);

            _filterService = filterService;
            _logTableService = logTableService;

            FileBrowser = fileBrowserService.FileBrowserModel;
            Filter = filterService.Filter;
            LogTable = logTableService.LogTable;
        }

        [Model(SupportIEditableObject = false)]
        [Expose("SelectedItems")]
        public FileBrowserModel FileBrowser { get; set; }

        [Model(SupportIEditableObject = false)]
        [Expose("Records")]
        [Expose("IsTimestampVisible")]
        public LogTable LogTable { get; private set; }

        [Model(SupportIEditableObject = false)]
        [Expose("IsUseDateRange")]
        [Expose("StartDate")]
        [Expose("EndDate")]
        public Filter Filter { get; set; }

        [Model(SupportIEditableObject = false)]
        [Expose("RegularExpression")]
        [ViewModelToModel("Filter")]
        public SearchTemplate SearchTemplate { get; set; }

        public void OnSelectedItemsChanged()
        {
            if (_prevSelectedItems is not null)
            {
                _prevSelectedItems.CollectionChanged -= OnSelectedItemsCollectionChanged;
            }

            if (FileBrowser.SelectedItems is not null)
            {
                FileBrowser.SelectedItems.CollectionChanged += OnSelectedItemsCollectionChanged;
            }

            _prevSelectedItems = FileBrowser.SelectedItems;
        }

#pragma warning disable AvoidAsyncVoid // Avoid async void
        private async void OnSelectedItemsCollectionChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
#pragma warning restore AvoidAsyncVoid // Avoid async void
        {
            await ApplyFilterAsync();
        }

        public void OnEndDateChanged()
        {
            _filterService.ApplyFilesFilter();
        }

        public void OnStartDateChanged()
        {
            _filterService.ApplyFilesFilter();
        }

        public void OnIsUseDateRangeChanged()
        {
            _filterService.ApplyFilesFilter();
        }

#pragma warning disable AvoidAsyncVoid // Avoid async void
        private async void OnSearchTemplateIsDirtyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
#pragma warning restore AvoidAsyncVoid // Avoid async void
        {
            if (Filter.UseTextSearch)
            {
                await ApplyFilterAsync(SearchTemplate);
            }
        }

#pragma warning disable AvoidAsyncVoid // Avoid async void
        private async void OnFilterIsDirtyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
#pragma warning restore AvoidAsyncVoid // Avoid async void
        {
            await ApplyFilterAsync(Filter);
        }


#pragma warning disable AvoidAsyncVoid // Avoid async void
        public async void OnSelectedItemChanged()
#pragma warning restore AvoidAsyncVoid // Avoid async void
        {
            await ApplyFilterAsync();
        }

        private async Task ApplyFilterAsync(SimplyClearableModel clearableModel = null)
        {
            await Task.Factory.StartNew(() =>
            {
                if (clearableModel is not null && !clearableModel.IsDirty)
                {
                    return;
                }

                _filterService.ApplyLogRecordsFilter();

                if (clearableModel is not null)
                {
                    clearableModel.MarkClean();
                }
            });
        }

        protected override async Task InitializeAsync()
        {
            SearchTemplate.PropertyChanged += OnSearchTemplateIsDirtyChanged;
            Filter.PropertyChanged += OnFilterIsDirtyChanged;

            var observable = Observable.FromEventPattern<PropertyChangedEventHandler, PropertyChangedEventArgs>(
                h => SearchTemplate.PropertyChanged += h,
                h => SearchTemplate.PropertyChanged -= h);

            _applyFilterListener?.Dispose();
            _applyFilterListener = observable
                .Delay(TimeSpan.FromMilliseconds(500))
                .Throttle(TimeSpan.FromMilliseconds(500))
                .ObserveOnDispatcher()
#pragma warning disable AvoidAsyncVoid // Avoid async void
                .Subscribe(async e =>
#pragma warning restore AvoidAsyncVoid // Avoid async void
                {
                    if (Filter.UseTextSearch)
                    {
                        await ApplyFilterAsync(SearchTemplate);
                    }
                });

            await base.InitializeAsync();
        }

        protected override async Task CloseAsync()
        {
            SearchTemplate.PropertyChanged -= OnSearchTemplateIsDirtyChanged;
            Filter.PropertyChanged -= OnFilterIsDirtyChanged;

            _applyFilterListener.Dispose();

            await base.CloseAsync();
        }
    }
}
