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

            ShowSettings = new Command(OnShowSettingsExecute);
            ShowKeyboardMappings = new Command(OnShowKeyboardMappingsExecute);

            Exit = new Command(OnExitExecute);

            commandManager.RegisterCommand(Commands.Settings.General, ShowSettings, this);
            commandManager.RegisterCommand(Commands.File.Exit, Exit, this);

            Title = AssemblyHelper.GetEntryAssembly().Title();
        }
        #endregion

        #region Properties
        [Model]
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

        private async void OnSaveWorkspaceExecute()
        {
            await _workspaceManager.StoreAndSave();
        }

        public Command CreateWorkspace { get; private set; }

        private async void OnCreateWorkspaceExecute()
        {
            var workspace = new Workspace();

            if (await _uiVisualizerService.ShowDialog<WorkspaceViewModel>(workspace) ?? false)
            {
                _workspaceManager.Add(workspace, true);

                await _workspaceManager.StoreAndSave();
            }
        }

        public Command ShowSettings { get; private set; }

        private async void OnShowSettingsExecute()
        {
            await _uiVisualizerService.ShowDialog<SettingsViewModel>();
        }

        public Command ShowKeyboardMappings { get; private set; }

        private async void OnShowKeyboardMappingsExecute()
        {
            await _uiVisualizerService.ShowDialog<KeyboardMappingsCustomizationViewModel>();
        }

        public Command Exit { get; private set; }

        private void OnExitExecute()
        {
            _navigationService.CloseApplication();
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

        protected override async Task Initialize()
        {
            await base.Initialize();

            SearchTemplate.PropertyChanged += OnSearchTemplatePropertyChanged;

            _workspaceManager.WorkspaceUpdated += OnCurrentWorkspaceChanged;

            UpdateCurrentWorkspace();
        }

        protected override async Task Close()
        {
            SearchTemplate.PropertyChanged -= OnSearchTemplatePropertyChanged;

            _workspaceManager.WorkspaceUpdated -= OnCurrentWorkspaceChanged;

            await base.Close();
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