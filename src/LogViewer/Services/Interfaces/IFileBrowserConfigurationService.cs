// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IFileBrowserConfigurationService.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace LogViewer.Services
{
    using System.Collections.Generic;

    public interface IFileBrowserConfigurationService
    {
        IEnumerable<string> LoadFolders();
        bool AddFolder(string fullPath);
        void RemoveFolder(string fullPath);
    }
}