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
        void BeginDirectoryWatching(FolderNode folder, EventHandler<FolderNodeEventArgs> folderContentChangedCallback);
        void EndDirectoryWatching(FolderNode folder);
    }
}