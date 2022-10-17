namespace LogViewer.ViewModels
{
    using System.ComponentModel;
    using System.Threading.Tasks;
    using Catel;
    using Catel.Configuration;
    using Catel.Fody;
    using Catel.Reflection;
    using Catel.MVVM;
    using Catel.Services;
    using Models;
    using Orc.WorkspaceManagement;
    using Orc.WorkspaceManagement.ViewModels;
    using Orchestra.ViewModels;
    using Services;
    using System;

    public class RibbonViewModel : ViewModelBase
    {
        private readonly IRegexService _regexService;
        private readonly INavigationService _navigationService;
        private readonly IConfigurationService _configurationService;
        private readonly IUIVisualizerService _uiVisualizerService;
        private readonly IWorkspaceManager _workspaceManager;
        private readonly IBusyIndicatorService _busyIndicatorService;
        private readonly IFilterService _filterService;

        public RibbonViewModel(IRegexService regexService, ICommandManager commandManager, 
            INavigationService navigationService, IConfigurationService configurationService, IUIVisualizerService uiVisualizerService,
            IWorkspaceManager workspaceManager, IBusyIndicatorService busyIndicatorService, IFilterService filterService)
        {
            ArgumentNullException.ThrowIfNull(regexService);
            ArgumentNullException.ThrowIfNull(commandManager);
            ArgumentNullException.ThrowIfNull(navigationService);
            ArgumentNullException.ThrowIfNull(configurationService);
            ArgumentNullException.ThrowIfNull(uiVisualizerService);
            ArgumentNullException.ThrowIfNull(workspaceManager);
            ArgumentNullException.ThrowIfNull(busyIndicatorService);
            ArgumentNullException.ThrowIfNull(filterService);

            _regexService = regexService;
            Filter = filterService.Filter;
            _navigationService = navigationService;
            _configurationService = configurationService;
            _uiVisualizerService = uiVisualizerService;
            _workspaceManager = workspaceManager;
            _busyIndicatorService = busyIndicatorService;
            _filterService = filterService;

            SaveWorkspace = new TaskCommand(OnSaveWorkspaceExecuteAsync, OnSaveWorkspaceCanExecute);
            CreateWorkspace = new TaskCommand(OnCreateWorkspaceExecuteAsync);

            ShowKeyboardMappings = new TaskCommand(OnShowKeyboardMappingsExecuteAsync);

            Title = AssemblyHelper.GetEntryAssembly().Title();
        }
    
        [Model(SupportIEditableObject = false)]
        [Expose("StartDate")]
        [Expose("EndDate")]
        [Expose("ShowInfo")]
        [Expose("ShowDebug")]
        [Expose("ShowWarning")]
        [Expose("ShowError")]
        [Expose("UseTextSearch")]
        [Expose("IsUseDateRange")]
        public Filter Filter { get; set; }

        [ViewModelToModel("Filter")]
        public SearchTemplate SearchTemplate { get; set; }

        public IWorkspace CurrentWorkspace { get; private set; }
  
        public TaskCommand SaveWorkspace { get; private set; }

        private bool OnSaveWorkspaceCanExecute()
        {
            var workspace = CurrentWorkspace;
            if (workspace is null)
            {
                return false;
            }

            if (string.Equals(workspace.Title, Workspaces.DefaultWorkspaceName))
            {
                return false;
            }

            return true;
        }

        private Task OnSaveWorkspaceExecuteAsync()
        {
            return _workspaceManager.StoreAndSaveAsync();
        }

        public TaskCommand CreateWorkspace { get; private set; }

        private async Task OnCreateWorkspaceExecuteAsync()
        {
            var workspace = new Workspace();

            var result = await _uiVisualizerService.ShowDialogAsync<WorkspaceViewModel>(workspace);
            if (result.DialogResult ?? false)
            {
                await _workspaceManager.AddAsync(workspace, true);
                await _workspaceManager.StoreAndSaveAsync();
            }
        }

        public TaskCommand ShowKeyboardMappings { get; private set; }

        private async Task OnShowKeyboardMappingsExecuteAsync()
        {
            await _uiVisualizerService.ShowDialogAsync<KeyboardMappingsCustomizationViewModel>();
        }

        private void OnSearchTemplatePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (string.Equals(e.PropertyName, "RegularExpression"))
            {
                return;
            }

            SearchTemplate.RegularExpression = SearchTemplate.IsEmpty() ? string.Empty : _regexService.ConvertToRegex(SearchTemplate.TemplateString, SearchTemplate.MatchCase, SearchTemplate.MatchWholeWord);
        }

        protected override async Task InitializeAsync()
        {
            await base.InitializeAsync();

            SearchTemplate.PropertyChanged += OnSearchTemplatePropertyChanged;

            _workspaceManager.WorkspaceUpdated += OnCurrentWorkspaceChanged;

            UpdateCurrentWorkspace();
        }

        protected override async Task CloseAsync()
        {
            SearchTemplate.PropertyChanged -= OnSearchTemplatePropertyChanged;

            _workspaceManager.WorkspaceUpdated -= OnCurrentWorkspaceChanged;

            await base.CloseAsync();
        }

        private void OnCurrentWorkspaceChanged(object sender, WorkspaceUpdatedEventArgs e)
        {
            UpdateCurrentWorkspace();
        }

        private void UpdateCurrentWorkspace()
        {
            CurrentWorkspace = _workspaceManager.Workspace;
        }
    }
}
