namespace LogViewer.Configuration
{
    using System;
    using Catel;
    using Catel.Configuration;
    using Models;
    using Services;

    public class TimestampVisibilityConfigurationSynchronizer
    {
        private readonly ILogTableConfigurationService _logTableConfigurationService;
        private readonly LogTable _logTable;

        public TimestampVisibilityConfigurationSynchronizer(IConfigurationService configurationService, ILogTableService logTableService,
            ILogTableConfigurationService logTableConfigurationService)
        {
            ArgumentNullException.ThrowIfNull(configurationService);
            ArgumentNullException.ThrowIfNull(logTableService);
            ArgumentNullException.ThrowIfNull(logTableConfigurationService);

            _logTableConfigurationService = logTableConfigurationService;

            _logTable = logTableService.LogTable;

            configurationService.ConfigurationChanged += OnConfigurationChanged;

            ApplyConfiguration();
        }

        private void OnConfigurationChanged(object sender, ConfigurationChangedEventArgs e)
        {
            ApplyConfiguration();
        }

        private void ApplyConfiguration()
        {
            var isTimestampVisibile = _logTableConfigurationService.GetIsTimestampVisibile();
            if (_logTable.IsTimestampVisible != isTimestampVisibile)
            {
                _logTable.IsTimestampVisible = isTimestampVisibile;
            }
        }
    }
}
