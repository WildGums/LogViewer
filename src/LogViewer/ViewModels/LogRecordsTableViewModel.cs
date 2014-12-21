// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogRecordsTableViewModel.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace LogViewer.ViewModels
{
    using System.Collections.ObjectModel;
    using System.ComponentModel;
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
        private readonly IFilterService _filterService;

        public LogRecordsTableViewModel(LogViewerModel logViewerModel, IFilterService filterService, ICommandManager commandManager)
        {
            Argument.IsNotNull(() => logViewerModel);
            Argument.IsNotNull(() => filterService);
            Argument.IsNotNull(() => commandManager); 

            _filterService = filterService;
            LogViewer = logViewerModel;
            LogRecords = new ObservableCollection<LogRecord>();

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

        [Model]
        [ViewModelToModel("LogViewer")]
        public Filter Filter { get; set; }

        public ObservableCollection<LogRecord> LogRecords { get; set; }

        [Model]
        [Expose("RegularExpression")]
        [ViewModelToModel("Filter")]
        public SearchTemplate SearchTemplate { get; set; }

        private void OnSearchTemplateIsDirtyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if (Filter.UseTextSearch)
            ApplyFilter(SearchTemplate);
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
            /*if (clearableModel != null && !clearableModel.IsDirty)
            {
                return;
            }

            LogRecords.ReplaceRange(_filterService.FilterRecords(LogViewer.Filter, SelectedItem));

            if (clearableModel != null)
            {
                clearableModel.MarkClean();
            }*/
        }
    }
}