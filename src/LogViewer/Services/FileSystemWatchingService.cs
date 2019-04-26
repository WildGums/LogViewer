// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileSystemWatchingService.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace LogViewer.Services
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.IO;
    using Catel;
    using Models;

    public class FileSystemWatchingService : IFileSystemWatchingService
    {
        #region Fields
        private readonly IDictionary<string, FileSystemWatcher> _fileSystemWatchers = new ConcurrentDictionary<string, FileSystemWatcher>();
        #endregion

        #region Methods
        public void BeginDirectoryWatching(string fullPath, string filter)
        {
            Argument.IsNotNullOrEmpty(() => fullPath);
            Argument.IsNotNullOrEmpty(() => filter);

            var fileSystemWatcher = new FileSystemWatcher();

            fileSystemWatcher.Path = fullPath;
            fileSystemWatcher.NotifyFilter = NotifyFilters.CreationTime | NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName;

            fileSystemWatcher.IncludeSubdirectories = true;
            fileSystemWatcher.Filter = filter;

            SubscribeWatcherEvents(fileSystemWatcher);

            fileSystemWatcher.EnableRaisingEvents = true;

            _fileSystemWatchers[fullPath] = fileSystemWatcher;
        }

        public void EndDirectoryWatching(string fullPath)
        {
            Argument.IsNotNullOrEmpty(() => fullPath);

            FileSystemWatcher fileSystemWatcher;
            var fullName = fullPath;

            if (_fileSystemWatchers.TryGetValue(fullName, out fileSystemWatcher))
            {
                fileSystemWatcher.EnableRaisingEvents = false;
                UnsubscribeWatcherEvents(fileSystemWatcher);
                fileSystemWatcher.Dispose();

                _fileSystemWatchers.Remove(fullName);
            }
        }

        public event EventHandler<FolderNodeEventArgs> ContentChanged;

        public void UpdateFilter(string filter)
        {
            foreach (var fileSystemWatcher in _fileSystemWatchers.Values)
            {
                fileSystemWatcher.Filter = filter;
            }
        }

        private void SubscribeWatcherEvents(FileSystemWatcher fileSystemWatcher)
        {
            Argument.IsNotNull(() => fileSystemWatcher);

            fileSystemWatcher.Renamed += OnRenamed;
            fileSystemWatcher.Created += OnChanged;
            fileSystemWatcher.Deleted += OnChanged;
            fileSystemWatcher.Changed += OnChanged;
        }

        private void UnsubscribeWatcherEvents(FileSystemWatcher fileSystemWatcher)
        {
            Argument.IsNotNull(() => fileSystemWatcher);

            fileSystemWatcher.Renamed -= OnRenamed;
            fileSystemWatcher.Created -= OnChanged;
            fileSystemWatcher.Deleted -= OnChanged;
            fileSystemWatcher.Changed -= OnChanged;
        }

        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType.HasFlag(WatcherChangeTypes.Created))
            {
                ContentChanged?.Invoke(this, new FolderNodeEventArgs(WatcherChangeTypes.Created, null, e.FullPath));
            }

            if (e.ChangeType.HasFlag(WatcherChangeTypes.Deleted))
            {
                ContentChanged?.Invoke(this, new FolderNodeEventArgs(WatcherChangeTypes.Deleted, e.FullPath, null));
            }

            if (e.ChangeType.HasFlag(WatcherChangeTypes.Changed))
            {
                ContentChanged?.Invoke(this, new FolderNodeEventArgs(WatcherChangeTypes.Changed, e.FullPath, e.FullPath));
            }
        }

        private void OnRenamed(object sender, RenamedEventArgs e)
        {
            ContentChanged?.Invoke(this, new FolderNodeEventArgs(WatcherChangeTypes.Renamed, e.OldFullPath, e.FullPath));
        }
        #endregion
    }
}
