namespace LogViewer.Services
{
    using System;
    using Catel.Configuration;

    public class LogTableConfigurationService : ILogTableConfigurationService
    {
        private const string IsTimestampVisible = "IsTimestampVisible";
        private const bool IsTimestampVisibleDefaultValue = true;
        private readonly IConfigurationService _configurationService;

        public LogTableConfigurationService(IConfigurationService configurationService)
        {
            ArgumentNullException.ThrowIfNull(configurationService);

            _configurationService = configurationService;
        }

        public bool GetIsTimestampVisible()
        {
            var stringValue = _configurationService.GetRoamingValue(IsTimestampVisible, IsTimestampVisibleDefaultValue.ToString());
            return bool.Parse(stringValue);
        }

        public void SetIsTimestampVisible(bool value)
        {
            _configurationService.SetRoamingValue(IsTimestampVisible, value.ToString());
        }
    }
}
