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
    using System.Threading.Tasks;

    using Catel;
    using Catel.Fody;
    using Catel.MVVM;

    using LogViewer.Models;
    using LogViewer.Models.Base;
    using LogViewer.Services;

    public class LogRecordsTableViewModel : ViewModelBase
    {
        #region Fields
        private readonly IFilterService _filterService;

        private ObservableCollection<NavigationNode> _prevSelectedItems;
        #endregion

        #region Constructors
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
        #endregion

        #region Properties
        public Command ResetSearchTemplate { get; private set; }

        [Model]
        [Expose("LogRecords")]
        public LogViewerModel LogViewer { get; set; }

        [ViewModelToModel("LogViewer")]
        public ObservableCollection<NavigationNode> SelectedItems { get; set; }

        [Model]
        [Expose("UseDateRange")]
        [Expose("StartDate")]
        [Expose("EndDate")]
        [ViewModelToModel("LogViewer")]
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
            _filterService.ApplyFilesFilter(LogViewer);
        }

        public void OnStartDateChanged()
        {
            _filterService.ApplyFilesFilter(LogViewer);
        }

        public void OnUseDateRangeChanged()
        {
            _filterService.ApplyFilesFilter(LogViewer);
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

            _filterService.ApplyLogRecodsFilter(LogViewer);

            if (clearableModel != null)
            {
                clearableModel.MarkClean();
            }
        }
        #endregion
    }
}