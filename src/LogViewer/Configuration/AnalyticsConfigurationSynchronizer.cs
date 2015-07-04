// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AnalyticsConfigurationSynchronizer.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace LogViewer.Configuration
{
    using Catel;
    using Catel.Configuration;
    using Orc.Analytics;

    internal class AnalyticsConfigurationSynchronizer
    {
        #region Constructors
        public AnalyticsConfigurationSynchronizer(IConfigurationService configurationService, IGoogleAnalyticsService googleAnalyticsService)
        {
            Argument.IsNotNull(() => configurationService);
            Argument.IsNotNull(() => googleAnalyticsService);

            _configurationService = configurationService;
            _googleAnalyticsService = googleAnalyticsService;

            _configurationService.ConfigurationChanged += OnConfigurationChanged;

            ApplyConfiguration();
        }
        #endregion

        #region Fields
        private readonly IConfigurationService _configurationService;
        private readonly IGoogleAnalyticsService _googleAnalyticsService;
        #endregion

        #region Methods
        private void OnConfigurationChanged(object sender, ConfigurationChangedEventArgs e)
        {
            ApplyConfiguration();
        }

        private void ApplyConfiguration()
        {
            _googleAnalyticsService.IsEnabled = _configurationService.GetValue(Settings.Application.General.EnableAnalytics,
                Settings.Application.General.EnableAnalyticsDefaultValue);
        }
        #endregion
    }
}