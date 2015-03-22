// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileSystemWatchingService.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace LogViewer.Services
{
    using System;
    using Models;

    public class FileSystemWatchingService : IFileSystemWatchingService
    {
        public void BeginDirectoryWatching(FolderNode folder, EventHandler<FolderNodeEventArgs> folderContentChangedCallback)
        {
           // TODO: get logic from the FolderNode
        }

        public void EndDirectoryWatching(FolderNode folder)
        {
            // TODO: get logic from the FolderNode
        }
    }
}