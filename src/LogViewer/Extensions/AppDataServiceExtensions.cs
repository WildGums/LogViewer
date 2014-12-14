// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AppDataServiceExtensions.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace LogViewer.Extensions
{
    using Catel.IO;
    using Orchestra.Services;

    public static class AppDataServiceExtensions
    {
        public static string GetRootAppDataFolder(this IAppDataService appDataService)
        {
            var currentCompanyDir = Path.GetParentDirectory(appDataService.ApplicationDataDirectory);
            return Path.GetParentDirectory(currentCompanyDir);
        }
    }
}