// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IManageUserDataService.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
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