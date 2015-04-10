// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IFileSystemServiceExtensions.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace LogViewer.Services
{
    using System.Threading.Tasks;
    using Models;

    public static class IFileSystemServiceExtensions
    {
        public static async Task<FolderNode> LoadFileSystemContentAsync(this IFileSystemService fileSystemService, string path, bool isNavigationRoot = false)
        {
            return await Task.Factory.StartNew(() => fileSystemService.LoadFileSystemContent(path, isNavigationRoot));
        }
    }
}