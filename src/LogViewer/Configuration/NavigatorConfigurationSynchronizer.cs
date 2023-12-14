namespace LogViewer.Configuration
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Catel.Configuration;
    using Models;
    using Services;

    public class NavigatorConfigurationSynchronizer
    {
        private readonly IFileBrowserConfigurationService _fileBrowserConfigurationService;
        private readonly IFileBrowserService _fileBrowserService;
        private readonly IFileSystemService _fileSystemService;

        public NavigatorConfigurationSynchronizer(IConfigurationService configurationService, IFileBrowserConfigurationService fileBrowserConfigurationService, IFileSystemService fileSystemService,
            IFileBrowserService fileBrowserService)
        {
            ArgumentNullException.ThrowIfNull(configurationService);
            ArgumentNullException.ThrowIfNull(fileSystemService);
            ArgumentNullException.ThrowIfNull(fileBrowserConfigurationService);
            ArgumentNullException.ThrowIfNull(fileBrowserService);

            _fileBrowserConfigurationService = fileBrowserConfigurationService;
            _fileSystemService = fileSystemService;
            _fileBrowserService = fileBrowserService;

            configurationService.ConfigurationChanged += OnConfigurationChanged;

            ApplyConfiguration();
        }

        private void OnConfigurationChanged(object sender, ConfigurationChangedEventArgs e)
        {
            ApplyConfiguration();
        }

        private void ApplyConfiguration()
        {
            var foldersFromConfig = _fileBrowserConfigurationService.LoadFolders().ToArray();
            var fileBrowserModel = _fileBrowserService.FileBrowserModel;

            var folderNodes = fileBrowserModel.RootDirectories;
            var foldersFromNavigator = folderNodes.Select(x => x.FullName).ToArray();

            UpdateFolderNodes(foldersFromConfig, foldersFromNavigator, folderNodes);
        }

        private void UpdateFolderNodes(string[] foldersFromConfig, string[] foldersFromNavigator, ObservableCollection<FolderNode> directories)
        {
            ArgumentNullException.ThrowIfNull(foldersFromConfig);
            ArgumentNullException.ThrowIfNull(foldersFromNavigator);
            ArgumentNullException.ThrowIfNull(directories);

            var newFolderNodes = foldersFromConfig.Except(foldersFromNavigator).Select(folder => _fileSystemService.LoadFileSystemContent(folder, true));
            var foldersToRemove = foldersFromNavigator.Except(foldersFromConfig);

            foreach (var folder in foldersToRemove)
            {
                var folderNode = directories.FirstOrDefault(x => string.Equals(x.FullName.ToLower(), folder.ToLower()));
                _fileSystemService.ReleaseFileSystemContent(folderNode);
                directories.Remove(folderNode);
            }

            foreach (var folderNode in newFolderNodes)
            {
                directories.Add(folderNode);
            }
        }
    }
}
