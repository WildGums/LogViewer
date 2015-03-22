// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogRecordsTableViewModel.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2014 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace LogViewer.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Reactive;
    using System.Reactive.Linq;
    using System.Threading.Tasks;

    using Catel;
    using Catel.Fody;
    using Catel.MVVM;

    using LogViewer.Models;
    using LogViewer.Services;

    public class LogRecordsTableViewModel : ViewModelBase
    {
        #region Fields
        private readonly IFilterService _filterService;

        private IDisposable _applyFilterListener;

        private ObservableCollection<NavigationNode> _prevSelectedItems;
        #endregion

        #region Constructors
        public LogRecordsTableViewModel(IFilterService filterService, ICommandManager commandManager, IFileBrowserService fileBrowserService)
        {
            Argument.IsNotNull(() => filterService);
            Argument.IsNotNull(() => commandManager);
            Argument.IsNotNull(() => fileBrowserService);

            _filterService = filterService;
            FileBrowser = fileBrowserService.FileBrowserModel;
            Filter = filterService.Filter;

            ResetSearchTemplate = new Command(OnResetSearchTemplateExecute);

            commandManager.RegisterCommand(Commands.Filter.ResetSearchTemplate, ResetSearchTemplate, this);
        }
        #endregion

        #region Properties
        public Command ResetSearchTemplate { get; private set; }

        [Model]
        [Expose("LogRecords")]
        [Expose("SelectedItems")]
        public FileBrowserModel FileBrowser { get; set; }

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

        private void OnSelectedItemsCollectionChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            ApplyFilter();
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

            _filterService.ApplyLogRecordsFilter(FileBrowser);

            if (clearableModel != null)
            {
                clearableModel.MarkClean();
            }
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
                .Subscribe(e =>
                {
                    if (Filter.UseTextSearch)
                    {
                        ApplyFilter(SearchTemplate);
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