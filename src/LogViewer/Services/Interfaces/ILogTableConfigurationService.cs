// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ILogTableConfigurationService.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace LogViewer.Services
{
    public interface ILogTableConfigurationService
    {
        #region Methods
        bool GetIsTimestampVisibile();
        void SetIsTimestampVisibile(bool value);
        #endregion
    }
}