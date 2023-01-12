namespace LogViewer.ViewModels
{
    using System;
    using System.Threading.Tasks;
    using Catel;
    using Catel.Configuration;
    using Catel.MVVM;
    using Orchestra;
    using Orc.Squirrel;

    public class StatusBarViewModel : ViewModelBase
    {
        private readonly IConfigurationService _configurationService;
        private readonly IUpdateService _updateService;

        public StatusBarViewModel(IConfigurationService configurationService, IUpdateService updateService)
        {
            ArgumentNullException.ThrowIfNull(configurationService);
            ArgumentNullException.ThrowIfNull(updateService);

            _configurationService = configurationService;
            _updateService = updateService;
        }

        public string ReceivingAutomaticUpdates { get; private set; }

        public bool IsUpdatedInstalled { get; private set; }

        public string Version { get; private set; }

        protected override async Task InitializeAsync()
        {
            await base.InitializeAsync();

            _configurationService.ConfigurationChanged += OnConfigurationChanged;
            _updateService.UpdateInstalled += OnUpdateInstalled;

            IsUpdatedInstalled = _updateService.IsUpdatedInstalled;
            Version = VersionHelper.GetCurrentVersion();

            await UpdateAutoUpdateInfoAsync();
        }

        protected override async Task CloseAsync()
        {
            _configurationService.ConfigurationChanged -= OnConfigurationChanged;
            _updateService.UpdateInstalled -= OnUpdateInstalled;

            await base.CloseAsync();
        }

        private async void OnConfigurationChanged(object sender, ConfigurationChangedEventArgs e)
        {
            if (e.Key.Contains("Updates"))
            {
                await UpdateAutoUpdateInfoAsync();
            }
        }

        private void OnUpdateInstalled(object sender, EventArgs e)
        {
            IsUpdatedInstalled = _updateService.IsUpdatedInstalled;
        }

        private async Task UpdateAutoUpdateInfoAsync()
        {
            string updateInfo = string.Empty;

            var checkForUpdates = _updateService.IsCheckForUpdatesEnabled;
            if (!_updateService.IsUpdateSystemAvailable || !checkForUpdates)
            {
                updateInfo = "Automatic updates are disabled";
            }
            else
            {
                var channel = _updateService.CurrentChannel;
                updateInfo = string.Format("Automatic updates are enabled for {0} versions", channel.Name.ToLower());
            }

            ReceivingAutomaticUpdates = updateInfo;
        }
    }
}
