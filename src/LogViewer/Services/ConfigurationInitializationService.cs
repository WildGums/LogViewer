namespace LogViewer.Services
{
    using System;
    using Catel.Configuration;

    internal class ConfigurationInitializationService : IConfigurationInitializationService
    {
        private readonly IConfigurationService _configurationService;

        public ConfigurationInitializationService(IConfigurationService configurationService)
        {
            ArgumentNullException.ThrowIfNull(configurationService);

            _configurationService = configurationService;
        }

        public void Initialize()
        {
        }

        private void InitializeConfigurationKey(string key, object defaultValue)
        {
            
        }
    }
}
