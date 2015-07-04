// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogTableConfigurationService.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
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
            var stringValue = _configurationService.GetValue(IsTimestampVisibile, IsTimestampVisibileDefaulValue.ToString());
            return bool.Parse(stringValue);
        }

        public void SetIsTimestampVisibile(bool value)
        {
            _configurationService.SetValue(IsTimestampVisibile, value.ToString());
        }
        #endregion
    }
}