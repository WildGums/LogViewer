// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StatusBarViewModel.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


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
        #region Fields
        private readonly IConfigurationService _configurationService;
        private readonly IUpdateService _updateService;
        #endregion

        #region Constructors
        public StatusBarViewModel(IConfigurationService configurationService, IUpdateService updateService)
        {
            Argument.IsNotNull(() => configurationService);
            Argument.IsNotNull(() => updateService);

            _configurationService = configurationService;
            _updateService = updateService;
        }
        #endregion

        #region Properties
        public string ReceivingAutomaticUpdates { get; private set; }

        public bool IsUpdatedInstalled { get; private set; }

        public string Version { get; private set; }
        #endregion

        #region Methods
        protected override async Task InitializeAsync()
        {
            await base.InitializeAsync();

            _configurationService.ConfigurationChanged += OnConfigurationChanged;
            _updateService.UpdateInstalled += OnUpdateInstalled;

            IsUpdatedInstalled = _updateService.IsUpdatedInstalled;
            Version = VersionHelper.GetCurrentVersion();

            UpdateAutoUpdateInfo();
        }

        protected override async Task CloseAsync()
        {
            _configurationService.ConfigurationChanged -= OnConfigurationChanged;
            _updateService.UpdateInstalled -= OnUpdateInstalled;

            await base.CloseAsync();
        }

        private void OnConfigurationChanged(object sender, ConfigurationChangedEventArgs e)
        {
            if (e.Key.Contains("Updates"))
            {
                UpdateAutoUpdateInfo();
            }
        }

        private void OnUpdateInstalled(object sender, EventArgs e)
        {
            IsUpdatedInstalled = _updateService.IsUpdatedInstalled;
        }

        private void UpdateAutoUpdateInfo()
        {
            string updateInfo = string.Empty;

            var checkForUpdates = _updateService.CheckForUpdates;
            if (!_updateService.IsUpdateSystemAvailable || !checkForUpdates)
            {
                updateInfo = "Automatic updates are disabled";
            }
            else
            {
                var channel = _updateService.CurrentChannel.Name;
                updateInfo = string.Format("Automatic updates are enabled for {0} versions", channel.ToLower());
            }

            ReceivingAutomaticUpdates = updateInfo;
        }
        #endregion
    }
}