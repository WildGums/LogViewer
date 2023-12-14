namespace LogViewer.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Catel.Configuration;
    using Catel.MVVM;
    using Orc.Squirrel;
    using Orc.WorkspaceManagement;
    using Orchestra.Services;
    using Services;
    using Settings = LogViewer.Settings;

    public class SettingsViewModel : ViewModelBase
    {
        private readonly IConfigurationService _configurationService;
        private readonly ILogTableConfigurationService _logTableConfigurationService;
        private readonly IManageAppDataService _manageAppDataService;
        private readonly IUpdateService _updateService;
        private readonly IWorkspaceManager _workspaceManager;

        public SettingsViewModel(IConfigurationService configurationService, IWorkspaceManager workspaceManager, IManageAppDataService manageAppDataService, IUpdateService updateService,
            ILogTableConfigurationService logTableConfigurationService)
        {
            ArgumentNullException.ThrowIfNull(configurationService);
            ArgumentNullException.ThrowIfNull(workspaceManager);
            ArgumentNullException.ThrowIfNull(manageAppDataService);
            ArgumentNullException.ThrowIfNull(updateService);
            ArgumentNullException.ThrowIfNull(logTableConfigurationService);

            _configurationService = configurationService;
            _workspaceManager = workspaceManager;
            _manageAppDataService = manageAppDataService;
            _updateService = updateService;
            _logTableConfigurationService = logTableConfigurationService;

            OpenApplicationDataDirectory = new Command(OnOpenApplicationDataDirectoryExecute);
            BackupUserData = new TaskCommand(OnBackupUserDataExecuteAsync);

            Title = "Settings";
        }

        public bool IsUpdateSystemAvailable { get; private set; }
        public bool CheckForUpdates { get; set; }
        public List<UpdateChannel> AvailableUpdateChannels { get; private set; }
        public UpdateChannel UpdateChannel { get; set; }
        public bool EnableTooltips { get; set; }
        public bool IsTimestampVisible { get; set; }

        protected override async Task InitializeAsync()
        {
            await base.InitializeAsync();

            var workspace = _workspaceManager.Workspace;
            EnableTooltips = workspace.GetWorkspaceValue(Settings.Workspace.General.EnableTooltips, Settings.Workspace.General.EnableTooltipsDefaultValue);

            IsUpdateSystemAvailable = _updateService.IsUpdateSystemAvailable;
            CheckForUpdates = _updateService.IsCheckForUpdatesEnabled;
            AvailableUpdateChannels = new List<UpdateChannel>(_updateService.AvailableChannels);
            UpdateChannel = _updateService.CurrentChannel;

            IsTimestampVisible = _logTableConfigurationService.GetIsTimestampVisibile();
        }

        protected override async Task<bool> SaveAsync()
        {
            var workspace = _workspaceManager.Workspace;
            workspace.SetWorkspaceValue(Settings.Workspace.General.EnableTooltips, EnableTooltips);

            await _workspaceManager.StoreAndSaveAsync();

            _updateService.IsCheckForUpdatesEnabled = CheckForUpdates;
            _updateService.CurrentChannel = UpdateChannel;

            _logTableConfigurationService.SetIsTimestampVisibile(IsTimestampVisible);

            return await base.SaveAsync();
        }

        public Command OpenApplicationDataDirectory { get; private set; }

        private void OnOpenApplicationDataDirectoryExecute()
        {
            _manageAppDataService.OpenApplicationDataDirectory(Catel.IO.ApplicationDataTarget.UserRoaming);
        }

        public TaskCommand BackupUserData { get; private set; }

        private async Task OnBackupUserDataExecuteAsync()
        {
            await _manageAppDataService.BackupUserDataAsync(Catel.IO.ApplicationDataTarget.UserRoaming);
        }
    }
}
