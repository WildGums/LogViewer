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