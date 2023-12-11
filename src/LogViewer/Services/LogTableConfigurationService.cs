namespace LogViewer.Services
{
    using System;
    using Catel.Configuration;

    public class LogTableConfigurationService : ILogTableConfigurationService
    {
        private const string IsTimestampVisibile = "IsTimestampVisibile";
        private const bool IsTimestampVisibileDefaulValue = true;
        private readonly IConfigurationService _configurationService;

        public LogTableConfigurationService(IConfigurationService configurationService)
        {
            ArgumentNullException.ThrowIfNull(configurationService);

            _configurationService = configurationService;
        }

        public bool GetIsTimestampVisibile()
        {
            var stringValue = _configurationService.GetRoamingValue(IsTimestampVisibile, IsTimestampVisibileDefaulValue.ToString());
            return bool.Parse(stringValue);
        }

        public void SetIsTimestampVisibile(bool value)
        {
            _configurationService.SetRoamingValue(IsTimestampVisibile, value.ToString());
        }
    }
}
