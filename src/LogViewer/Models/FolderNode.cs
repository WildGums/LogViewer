// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FolderNode.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace LogViewer.Models
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Windows.Data;
    using Catel;
    using Catel.Collections;

    public class FolderNode : NavigationNode
    {
        #region Fields
        private readonly List<EventHandler<FolderNodeEventArgs>> _eventHandlers;
        private readonly FileSystemWatcher _fileSystemWatcher;
        private bool _disposed;
        #endregion

        #region Constructors
        public FolderNode(DirectoryInfo directoryInfo)
        {
            Argument.IsNotNull(() => directoryInfo);

            Name = directoryInfo.Name;

            FullName = directoryInfo.FullName;

            Files = new ObservableCollection<FileNode>();
            Directories = new ObservableCollection<FolderNode>();

            _eventHandlers = new List<EventHandler<FolderNodeEventArgs>>();

            _fileSystemWatcher = new FileSystemWatcher();
            _fileSystemWatcher.Path = FullName;
            _fileSystemWatcher.NotifyFilter = NotifyFilters.CreationTime | NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName;

            _fileSystemWatcher.Renamed += OnRenamed;
            _fileSystemWatcher.Created += OnChanged;
            _fileSystemWatcher.Deleted += OnChanged;

           // return; // temporary switched off
            _fileSystemWatcher.EnableRaisingEvents = true;
        }
        #endregion

        #region Properties
        public override bool AllowMultiSelection
        {
            get { return false; }
        }

        public IList<FileNode> Files { get; set; }
        public IList<FolderNode> Directories { get; set; }

        public IList Children
        {
            get { return new CompositeCollection {new CollectionContainer {Collection = Directories}, new CollectionContainer {Collection = Files}}; }
        }
        #endregion

        #region Methods
        private event EventHandler<FolderNodeEventArgs> _contentChanged;

        public event EventHandler<FolderNodeEventArgs> ContentChanged
        {
            add
            {
                _contentChanged += value;
                _eventHandlers.Add(value);
            }
            remove
            {
                _contentChanged -= value;
                _eventHandlers.Remove(value);
            }
        }

        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType.HasFlag(WatcherChangeTypes.Created))
            {
                _contentChanged.SafeInvoke(this, new FolderNodeEventArgs(WatcherChangeTypes.Created, null, e.FullPath));
            }

            if (e.ChangeType.HasFlag(WatcherChangeTypes.Deleted))
            {
                _contentChanged.SafeInvoke(this, new FolderNodeEventArgs(WatcherChangeTypes.Deleted, e.FullPath, null));
            }
        }

        private void OnRenamed(object sender, RenamedEventArgs e)
        {
            _contentChanged.SafeInvoke(this, new FolderNodeEventArgs(WatcherChangeTypes.Renamed, e.OldFullPath, e.FullPath));
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~FolderNode()
        {
            Dispose(false);
        }

        private void RemoveAllEvents()
        {
            foreach (var eh in _eventHandlers.ToArray())
            {
                ContentChanged -= eh;
            }

            _eventHandlers.Clear();
        }

        protected void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                RemoveAllEvents();
            }

            _disposed = true;
        }
        #endregion
    }
}