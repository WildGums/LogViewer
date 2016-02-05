// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IFileBrowserConfigurationService.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
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