// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RibbonViewModel.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2014 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


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

    public class RibbonViewModel : ViewModelBase
    {
        #region Fields
        private readonly IRegexService _regexService;
        private readonly INavigationService _navigationService;
        private readonly IConfigurationService _configurationService;
        private readonly IUIVisualizerService _uiVisualizerService;
        private readonly IWorkspaceManager _workspaceManager;
        private readonly IPleaseWaitService _pleaseWaitService;
        private readonly IFilterService _filterService;
        #endregion

        #region Constructors
        public RibbonViewModel(IRegexService regexService, ICommandManager commandManager, 
            INavigationService navigationService, IConfigurationService configurationService, IUIVisualizerService uiVisualizerService,
            IWorkspaceManager workspaceManager, IPleaseWaitService pleaseWaitService, IFilterService filterService)
        {
            Argument.IsNotNull(() => regexService);
            Argument.IsNotNull(() => commandManager);
            Argument.IsNotNull(() => navigationService);
            Argument.IsNotNull(() => configurationService);
            Argument.IsNotNull(() => uiVisualizerService);
            Argument.IsNotNull(() => workspaceManager);
            Argument.IsNotNull(() => pleaseWaitService);
            Argument.IsNotNull(() => filterService);

            _regexService = regexService;
            Filter = filterService.Filter;
            _navigationService = navigationService;
            _configurationService = configurationService;
            _uiVisualizerService = uiVisualizerService;
            _workspaceManager = workspaceManager;
            _pleaseWaitService = pleaseWaitService;
            _filterService = filterService;

            SaveWorkspace = new Command(OnSaveWorkspaceExecute, OnSaveWorkspaceCanExecute);
            CreateWorkspace = new Command(OnCreateWorkspaceExecute);

            ShowKeyboardMappings = new Command(OnShowKeyboardMappingsExecute);

            Title = AssemblyHelper.GetEntryAssembly().Title();
        }
        #endregion

        #region Properties
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
        #endregion

        #region Commands
        public Command SaveWorkspace { get; private set; }

        private bool OnSaveWorkspaceCanExecute()
        {
            var workspace = CurrentWorkspace;
            if (workspace == null)
            {
                return false;
            }

            if (string.Equals(workspace.Title, Workspaces.DefaultWorkspaceName))
            {
                return false;
            }

            return true;
        }

        private void OnSaveWorkspaceExecute()
        {
            _workspaceManager.StoreAndSave();
        }

        public Command CreateWorkspace { get; private set; }

        private void OnCreateWorkspaceExecute()
        {
            var workspace = new Workspace();

            if (_uiVisualizerService.ShowDialog<WorkspaceViewModel>(workspace) ?? false)
            {
                _workspaceManager.Add(workspace, true);

                _workspaceManager.StoreAndSave();
            }
        }

        public Command ShowKeyboardMappings { get; private set; }

        private async void OnShowKeyboardMappingsExecute()
        {
            _uiVisualizerService.ShowDialog<KeyboardMappingsCustomizationViewModel>();
        }

        #endregion

        #region Methods
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
        #endregion
    }
}