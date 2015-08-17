// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IManageUserDataService.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace LogViewer.Services
{
    using System.Threading.Tasks;
    using Orchestra.Services;

    public interface IManageUserDataService : IAppDataService
    {
        #region Methods
        Task<bool> ResetFiltersAsync();
        Task<bool> ResetWorkspacesAsync();
        #endregion
    }
}