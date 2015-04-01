// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileSystemService.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace LogViewer.Services
{
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;
    using Catel;
    using Catel.Services;
    using Models;
    using Orchestra.Models;

    internal class FileSystemService : IFileSystemService
    {
        #region Fields
        private readonly IDispatcherService _dispatcherService;
        private readonly IFileSystemWatchingService _fileSystemWatchingService;
        private readonly INavigationNodeCacheService _navigationNodeCacheService;
        private readonly IFilterService _filterService;
        private readonly IFileNodeService _fileNodeService;
        private string _regexFilter;
        private string _wildcardsFilter;
        #endregion

        #region Constructors
        public FileSystemService(IDispatcherService dispatcherService, IFileNodeService fileNodeService, IFileSystemWatchingService fileSystemWatchingService,
            INavigationNodeCacheService navigationNodeCacheService, IFilterService filterService)
        {
            Argument.IsNotNull(() => dispatcherService);
            Argument.IsNotNull(() => fileNodeService);
            Argument.IsNotNull(() => fileSystemWatchingService);
            Argument.IsNotNull(() => navigationNodeCacheService);
            Argument.IsNotNull(() => filterService);

            _dispatcherService = dispatcherService;
            _fileNodeService = fileNodeService;
            _fileSystemWatchingService = fileSystemWatchingService;
            _navigationNodeCacheService = navigationNodeCacheService;
            _filterService = filterService;

            Filter = "*.log";

            fileSystemWatchingService.ContentChanged += OnFolderContentChanged;
        }
        #endregion

        #region Methods
        public FolderNode LoadFileSystemContent(string path, bool isNavigationRoot = false)
        {
            Argument.IsNotNullOrEmpty(() => path);

            var directoryInfo = new DirectoryInfo(path);

            FolderNode folder = null;
            _dispatcherService.Invoke(() => { folder = new FolderNode(directoryInfo); });

            var fileInfos = Directory.GetFiles(path, _wildcardsFilter, SearchOption.TopDirectoryOnly).Where(x => x.IsSupportedFile(_regexFilter))
                .Select(fileName => LoadFileFromFileSystem(Path.Combine(path, fileName))).OrderByDescending(x => x.Name);
            _dispatcherService.Invoke(() => folder.Files = new ObservableCollection<FileNode>(fileInfos));

            foreach (var directory in Directory.GetDirectories(path))
            {
                var fullPath = Path.Combine(path, directory);
                _dispatcherService.Invoke(() => folder.Directories.Add(LoadFileSystemContent(fullPath)));
            }

            if (isNavigationRoot)
            {
                _fileSystemWatchingService.BeginDirectoryWatching(folder.FullName, _wildcardsFilter);
            }
            else
            {
                folder.UpdateVisibility();
            }

            _navigationNodeCacheService.AddToCache(folder);
            return folder;
        }

        public void ReleaseFileSystemContent(FolderNode folder)
        {
            _fileSystemWatchingService.EndDirectoryWatching(folder.FullName);
            OnDeleted(folder.FullName);
        }


        public string Filter
        {
            get { return _wildcardsFilter; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    _wildcardsFilter = string.Empty;
                    _regexFilter = string.Empty;
                }
                else
                {
                    _wildcardsFilter = value;
                    _regexFilter = _wildcardsFilter.ConvertWildcardToRegex();
                    _fileSystemWatchingService.UpdateFilter(_wildcardsFilter);
                }                
            }
        }

        private void OnRenamed(string newName, string oldName)
        {
            Argument.IsNotNullOrEmpty(() => oldName);
            Argument.IsNotNullOrEmpty(() => newName);

            if (newName.IsFile())
            {
                RenameFile(oldName, newName);
            }

            if (newName.IsDirectory())
            {
                RenameFolder(oldName, newName);
            }
        }

        private void OnCreated(string fullPath)
        {
            Argument.IsNotNullOrEmpty(() => fullPath);

            var folder = GetParentFolderNode(fullPath);

            if (fullPath.IsFile() && folder.Files.FirstOrDefault(x => string.Equals(x.FullName, fullPath)) == null)
            {
                if (fullPath.IsSupportedFile(_regexFilter))
                {
                    var fileNode = GetFromCacheOrLoad(fullPath);
                    folder.Files.Add(fileNode);
                    _navigationNodeCacheService.AddToCache(fileNode);
                }
            }

            if (fullPath.IsDirectory())
            {
                var folderNode = LoadFileSystemContent(fullPath);
                folderNode.IsVisible = false;
                folder.Directories.Add(folderNode);
                _navigationNodeCacheService.AddToCache(folderNode);
            }
            
            _filterService.ApplyFilesFilter();
        }

       

        private void OnDeleted(string fullPath)
        {
            Argument.IsNotNullOrEmpty(() => fullPath);

            var folder = GetParentFolderNode(fullPath);

            var childDir = folder.Directories.FirstOrDefault(x => string.Equals(x.FullName, fullPath));
            if (childDir != null)
            {
                folder.Directories.Remove(childDir);                
            }

            var childFile = folder.Files.FirstOrDefault(x => string.Equals(x.FullName, fullPath));
            if (childFile != null)
            {
                folder.Files.Remove(childFile);
            }

            _navigationNodeCacheService.RemoveFromCache(fullPath);
            _filterService.ApplyFilesFilter();
        }

        private void OnChanged(string fullPath)
        {
            var fileNode = GetFromCacheOrLoad(fullPath);
            _fileNodeService.ReloadFileNode(fileNode);

            _filterService.ApplyLogRecordsFilter(fileNode);
        }

        private FileNode GetFromCacheOrLoad(string fullPath)
        {
            var fileNode = _navigationNodeCacheService.GetFromCache<FileNode>(fullPath);
            if (fileNode == null)
            {
                fileNode = LoadFileFromFileSystem(fullPath);
            }
            return fileNode;
        }

        private void RenameFolder(string oldName, string newName)
        {
            Argument.IsNotNullOrEmpty(() => oldName);
            Argument.IsNotNullOrEmpty(() => newName);

            var fromCache = _navigationNodeCacheService.GetFromCache<FolderNode>(newName);
            if (fromCache != null)
            {
                return;
            }

            var folder = GetParentFolderNode(newName);

            var oldDir = folder.Directories.FirstOrDefault(x => string.Equals(x.FullName, oldName));
            if (oldDir == null)
            {
                return;
            }

            folder.Directories.Remove(oldDir);
            _navigationNodeCacheService.RemoveFromCache(oldName);

            ClearSubfolders(oldDir);

            if (Directory.Exists(newName))
            {
                var newDir = LoadFileSystemContent(newName);
                folder.Directories.Add(newDir);
                _navigationNodeCacheService.AddToCache(newDir);
            }
        }

        private void ClearSubfolders(FolderNode folder)
        {
            Argument.IsNotNull(() => folder);

            foreach (var folderNode in folder.Directories)
            {
                ClearSubfolders(folderNode);
            }

            folder.Directories.Clear();
        }

        private void RenameFile(string oldName, string newName)
        {
            Argument.IsNotNullOrEmpty(() => oldName);
            Argument.IsNotNullOrEmpty(() => newName);

            var fromCache = _navigationNodeCacheService.GetFromCache<FileNode>(newName);
            if (fromCache != null)
            {
                return;
            }

            var folder = GetParentFolderNode(newName);

            var fileNode = folder.Files.FirstOrDefault(x => string.Equals(x.FullName, oldName));
            if (!newName.IsSupportedFile(_regexFilter))
            {
                if (fileNode != null)
                {
                    folder.Files.Remove(fileNode);
                    _navigationNodeCacheService.RemoveFromCache(fileNode.FullName);
                }
                return;
            }

            if (fileNode == null)
            {
                var newFileNode = LoadFileFromFileSystem(newName);
                folder.Files.Add(newFileNode);
            }
            else
            {
                folder.Files.Remove(fileNode);
                _navigationNodeCacheService.RemoveFromCache(fileNode.FullName);
                fileNode.FileInfo = new FileInfo(newName);
                folder.Files.Add(fileNode);
                _navigationNodeCacheService.AddToCache(fileNode);
            }
        }

        private void OnFolderContentChanged(object sender, FolderNodeEventArgs e)
        {
            switch (e.ChangeType)
            {
                case WatcherChangeTypes.Changed:
                    _dispatcherService.Invoke(() => OnChanged(e.NewPath));
                    break;

                case WatcherChangeTypes.Created:
                    _dispatcherService.Invoke(() => OnCreated(e.NewPath));
                    break;

                case WatcherChangeTypes.Deleted:
                    _dispatcherService.Invoke(() => OnDeleted(e.OldPath));
                    break;

                case WatcherChangeTypes.Renamed:
                    _dispatcherService.Invoke(() => OnRenamed(e.NewPath, e.OldPath));
                    break;
            }
        }      

        private FileNode LoadFileFromFileSystem(string fullName)
        {
            Argument.IsNotNullOrEmpty(() => fullName);

            var fileNode = _fileNodeService.LoadFileNode(fullName);
            _navigationNodeCacheService.AddToCache(fileNode);
            return fileNode;
        }

        private FolderNode GetParentFolderNode(string fullPath)
        {
            var parentDirectory = Catel.IO.Path.GetParentDirectory(fullPath);
            var folder = _navigationNodeCacheService.GetFromCache<FolderNode>(parentDirectory);
            return folder;
        }

        #endregion
    }
}