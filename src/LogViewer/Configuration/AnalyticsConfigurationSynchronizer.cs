// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AnalyticsConfigurationSynchronizer.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
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
        public AnalyticsConfigurationSynchronizer(IConfigurationService configurationService, IAnalyticsService analyticsService)
        {
            Argument.IsNotNull(() => configurationService);
            Argument.IsNotNull(() => analyticsService);

            _configurationService = configurationService;
            _analyticsService = analyticsService;

            _configurationService.ConfigurationChanged += OnConfigurationChanged;

            ApplyConfiguration();
        }
        #endregion

        #region Fields
        private readonly IConfigurationService _configurationService;
        private readonly IAnalyticsService _analyticsService;
        #endregion

        #region Methods
        private void OnConfigurationChanged(object sender, ConfigurationChangedEventArgs e)
        {
            ApplyConfiguration();
        }

        private void ApplyConfiguration()
        {
            _analyticsService.IsEnabled = _configurationService.GetRoamingValue(Settings.Application.General.EnableAnalytics,
                Settings.Application.General.EnableAnalyticsDefaultValue);
        }
        #endregion
    }
}