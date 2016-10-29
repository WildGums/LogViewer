// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogTableConfigurationService.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace LogViewer.Services
{
    using Catel;
    using Catel.Configuration;

    public class LogTableConfigurationService : ILogTableConfigurationService
    {
        #region Fields
        private const string IsTimestampVisibile = "IsTimestampVisibile";
        private const bool IsTimestampVisibileDefaulValue = true;
        private readonly IConfigurationService _configurationService;
        #endregion

        #region Constructors
        public LogTableConfigurationService(IConfigurationService configurationService)
        {
            Argument.IsNotNull(() => configurationService);

            _configurationService = configurationService;
        }
        #endregion

        #region Methods
        public bool GetIsTimestampVisibile()
        {
            var stringValue = _configurationService.GetRoamingValue(IsTimestampVisibile, IsTimestampVisibileDefaulValue.ToString());
            return bool.Parse(stringValue);
        }

        public void SetIsTimestampVisibile(bool value)
        {
            _configurationService.SetRoamingValue(IsTimestampVisibile, value.ToString());
        }
        #endregion
    }
}