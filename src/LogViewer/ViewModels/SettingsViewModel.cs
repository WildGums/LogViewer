// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SettingsViewModel.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace LogViewer.ViewModels
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Catel;
    using Catel.Configuration;
    using Catel.MVVM;
    using LogViewer.Services;
    using Orc.Squirrel;
    using Orc.WorkspaceManagement;
    using Settings = LogViewer.Settings;

    public class SettingsViewModel : ViewModelBase
    {
        private readonly IConfigurationService _configurationService;
        private readonly IManageUserDataService _manageUserDataService;
        private readonly IUpdateService _updateService;
        private readonly IWorkspaceManager _workspaceManager;

        public SettingsViewModel(IConfigurationService configurationService, IWorkspaceManager workspaceManager, IManageUserDataService manageUserDataService, IUpdateService updateService)
        {
            Argument.IsNotNull(() => configurationService);
            Argument.IsNotNull(() => workspaceManager);
            Argument.IsNotNull(() => manageUserDataService);
            Argument.IsNotNull(() => updateService);

            _configurationService = configurationService;
            _workspaceManager = workspaceManager;
            _manageUserDataService = manageUserDataService;
            _updateService = updateService;

            OpenApplicationDataDirectory = new Command(OnOpenApplicationDataDirectoryExecute);
            BackupUserData = new Command(OnBackupUserDataExecute);
            ResetFilters = new Command(OnResetFiltersExecute);
            ResetWorkspaces = new Command(OnResetWorkspacesExecute);

            Title = "Settings";
        }

        #region Properties
        public bool IsUpdateSystemAvailable { get; private set; }

        public bool CheckForUpdates { get; set; }

        public List<UpdateChannel> AvailableUpdateChannels { get; private set; }

        public UpdateChannel UpdateChannel { get; set; }

        public bool EnableAnalytics { get; set; }

        public bool EnableTooltips { get; set; }
        #endregion

        #region Commands
        public Command OpenApplicationDataDirectory { get; private set; }

        private async void OnOpenApplicationDataDirectoryExecute()
        {
            await _manageUserDataService.OpenApplicationDataDirectory();
        }

        public Command BackupUserData { get; private set; }

        private async void OnBackupUserDataExecute()
        {
            await _manageUserDataService.BackupUserData();
        }

        public Command ResetFilters { get; private set; }

        private async void OnResetFiltersExecute()
        {
            await _manageUserDataService.ResetFilters();
        }

        public Command ResetWorkspaces { get; private set; }

        private async void OnResetWorkspacesExecute()
        {
            await _manageUserDataService.ResetWorkspaces();
        }
        #endregion

        #region Methods
        protected override async Task Initialize()
        {
            await base.Initialize();

            var workspace = _workspaceManager.Workspace;
            EnableTooltips = workspace.GetWorkspaceValue(Settings.Workspace.General.EnableTooltips, Settings.Workspace.General.EnableTooltipsDefaultValue);

            EnableAnalytics = _configurationService.GetValue<bool>(Settings.Application.General.EnableAnalytics);

            IsUpdateSystemAvailable = _updateService.IsUpdateSystemAvailable;
            CheckForUpdates = _updateService.CheckForUpdates;
            AvailableUpdateChannels = new List<UpdateChannel>(_updateService.AvailableChannels);
            UpdateChannel = _updateService.CurrentChannel;
        }

        protected override async Task<bool> Save()
        {
            var workspace = _workspaceManager.Workspace;
            workspace.SetWorkspaceValue(Settings.Workspace.General.EnableTooltips, EnableTooltips);

            await _workspaceManager.StoreAndSave();

            _configurationService.SetValue(Settings.Application.General.EnableAnalytics, EnableAnalytics);

            _updateService.CheckForUpdates = CheckForUpdates;
            _updateService.CurrentChannel = UpdateChannel;

            return await base.Save();
        }
        #endregion
    }
}