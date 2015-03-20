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
    using Catel.Collections;
    using Catel.Services;
    using Models;

    internal class FileSystemService : IFileSystemService
    {
        #region Fields
        private string _regexFilter;
        private string _wildcardsFilter;
        private readonly IDispatcherService _dispatcherService;
        private readonly ILogFileService _logFileService;
        #endregion

        #region Constructors
        public FileSystemService(IDispatcherService dispatcherService, ILogFileService logFileService)
        {
            Argument.IsNotNull(() => dispatcherService);
            Argument.IsNotNull(() => logFileService);

            _dispatcherService = dispatcherService;
            _logFileService = logFileService;

            SetFileSearchFilter("*.log");
        }
        #endregion

        #region Methods
        public void SetFileSearchFilter(string value)
        {
            Argument.IsNotNullOrEmpty(() => value);

            _wildcardsFilter = value;
            _regexFilter = _wildcardsFilter.ConvertWildcardToRegex();
        }

        public string GetFileSearchFilter()
        {
            return _wildcardsFilter;
        }

        public FolderNode LoadFileSystemContent(string path, bool isNavigationRoot = false)
        {
            Argument.IsNotNullOrEmpty(() => path);

            var directoryInfo = new DirectoryInfo(path);

            FolderNode folder = null;
            _dispatcherService.Invoke(() =>
            {
                folder = new FolderNode(directoryInfo) { IsNavigationRoot = isNavigationRoot };
            });
            

            var fileInfos = Directory.GetFiles(path, _wildcardsFilter, SearchOption.TopDirectoryOnly).Where(x => x.IsSupportedFile(_regexFilter)).Select(fileName => LoadFileFromFileSystem(Path.Combine(path, fileName)));
           _dispatcherService.Invoke(() => folder.Files = new ObservableCollection<FileNode>(fileInfos));

            foreach (var directory in Directory.GetDirectories(path))
            {
                var fullPath = Path.Combine(path, directory);
                _dispatcherService.Invoke(() => folder.Directories.Add(LoadFileSystemContent(fullPath)));
            }

            folder.ContentChanged += OnFolderContentChanged;

            return folder;
        }

        private void OnCreated(FolderNode folder, string fullPath)
        {
            Argument.IsNotNull(() => folder);
            Argument.IsNotNullOrEmpty(() => fullPath);

            if (File.Exists(fullPath))
            {
                if (fullPath.IsSupportedFile(_regexFilter))
                {
                    var fileInfo = LoadFileFromFileSystem(fullPath);
                    folder.Files.Add(fileInfo);
                }
            }

            if (Directory.Exists(fullPath))
            {
                var folderInfo = LoadFileSystemContent(fullPath);
                folder.Directories.Add(folderInfo);
            }
        }

        private static void OnDeleted(FolderNode folder, string fullPath)
        {
            Argument.IsNotNull(() => folder);
            Argument.IsNotNullOrEmpty(() => fullPath);

            var childDir = folder.Directories.FirstOrDefault(x => string.Equals(x.FullName, fullPath));
            if (childDir != null)
            {
                folder.Directories.Remove(childDir);
                childDir.Dispose();
            }

            var childFile = folder.Files.FirstOrDefault(x => string.Equals(x.FullName, fullPath));
            if (childFile != null)
            {
                folder.Files.Remove(childFile);
            }
        }

        private void OnRenamed(FolderNode folder, string newName, string oldName)
        {
            Argument.IsNotNull(() => folder);
            Argument.IsNotNullOrEmpty(() => oldName);
            Argument.IsNotNullOrEmpty(() => newName);

            if (File.Exists(newName))
            {
                RenameFile(folder, oldName, newName);
            }

            if (Directory.Exists(newName))
            {
                RenameFolder(folder, oldName, newName);
            }
        }

        private void RenameFolder(FolderNode folder, string oldName, string newName)
        {
            Argument.IsNotNull(() => folder);
            Argument.IsNotNullOrEmpty(() => oldName);
            Argument.IsNotNullOrEmpty(() => newName);

            var oldDir = folder.Directories.FirstOrDefault(x => string.Equals(x.FullName, oldName));
            if (oldDir == null)
            {
                return;
            }

            folder.Directories.Remove(oldDir);

            ClearSubfolders(oldDir);
            oldDir.Dispose();

            if (Directory.Exists(newName))
            {
                var newDir = LoadFileSystemContent(newName);
                folder.Directories.Add(newDir);
            }
        }

        private void ClearSubfolders(FolderNode folder)
        {
            Argument.IsNotNull(() => folder);

            foreach (var folderNode in folder.Directories)
            {
                folderNode.Dispose();
                ClearSubfolders(folderNode);
            }

            folder.Directories.Clear();
        }

        private void RenameFile(FolderNode folder, string oldName, string newName)
        {
            Argument.IsNotNull(() => folder);
            Argument.IsNotNullOrEmpty(() => oldName);
            Argument.IsNotNullOrEmpty(() => newName);

            var oldFile = folder.Files.FirstOrDefault(x => string.Equals(x.FullName, oldName));
            if (!newName.IsSupportedFile(_regexFilter))
            {
                if (oldFile != null)
                {
                    folder.Files.Remove(oldFile);
                }
                return;
            }

            if (oldFile == null)
            {
                var fileInfo = LoadFileFromFileSystem(newName);
                folder.Files.Add(fileInfo);
            }
            else
            {
                folder.Files.Remove(oldFile);
                oldFile.FileInfo = new FileInfo(newName);
                folder.Files.Add(oldFile);
            }
        }

        private void OnFolderContentChanged(object sender, FolderNodeEventArgs e)
        {
            var folder = sender as FolderNode;
            if (folder == null)
            {
                return;
            }

            if (e.ChangeType.HasFlag(WatcherChangeTypes.Deleted))
            {
                _dispatcherService.Invoke(() => OnDeleted(folder, e.OldPath));
            }

            if (e.ChangeType.HasFlag(WatcherChangeTypes.Created))
            {
                _dispatcherService.Invoke(() => OnCreated(folder, e.NewPath));
            }

            if (e.ChangeType.HasFlag(WatcherChangeTypes.Renamed))
            {
                _dispatcherService.Invoke(() => OnRenamed(folder, e.NewPath, e.OldPath));
            }
        }

        public FileNode LoadFileFromFileSystem(string fullName)
        {
            Argument.IsNotNullOrEmpty(() => fullName);

            var fileInfo = _logFileService.LoadLogFile(fullName);
            return fileInfo;
        }
        #endregion
    }
}