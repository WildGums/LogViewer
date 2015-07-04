// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IFileSystemWatchingService.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace LogViewer.Services
{
    using System;
    using Models;

    public interface IFileSystemWatchingService
    {
        #region Methods
        void BeginDirectoryWatching(string fullPath, string filter = null);
        void EndDirectoryWatching(string fullPath);
        event EventHandler<FolderNodeEventArgs> ContentChanged;
        void UpdateFilter(string filter);
        #endregion
    }
}