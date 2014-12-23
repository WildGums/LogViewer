// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AppDataServiceExtensions.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace LogViewer
{
    using Catel;
    using Catel.IO;

    using Orchestra.Services;

    public static class AppDataServiceExtensions
    {
        #region Methods
        public static string GetRootAppDataFolder(this IAppDataService appDataService)
        {
            Argument.IsNotNull(() => appDataService);

            var currentCompanyDir = Path.GetParentDirectory(appDataService.ApplicationDataDirectory);
            return Path.GetParentDirectory(currentCompanyDir);
        }
        #endregion
    }
}