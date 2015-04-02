// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ILogTableConfigurationService.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
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