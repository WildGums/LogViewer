﻿namespace LogViewer
{
    using System;
    using Catel.IO;
    using Catel.Services;

    public static class AppDataServiceExtensions
    {
        public static string GetRootAppDataFolder(this IAppDataService appDataService)
        {
            ArgumentNullException.ThrowIfNull(appDataService);

            var currentCompanyDir = Path.GetParentDirectory(appDataService.GetApplicationDataDirectory(ApplicationDataTarget.UserRoaming));
            return Path.GetParentDirectory(currentCompanyDir);
        }
    }
}
