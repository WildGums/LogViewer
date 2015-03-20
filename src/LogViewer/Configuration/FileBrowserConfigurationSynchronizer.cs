// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileBrowserConfigurationSynchronizer.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace LogViewer.Configuration
{
    using System.Collections.ObjectModel;
    using System.Linq;
    using Catel;
    using Catel.Collections;
    using Catel.Configuration;
    using Models;
    using Services;

    public class NavigatorConfigurationSynchronizer
    {
        #region Fields
        private readonly IFileBrowserConfigurationService _fileBrowserConfigurationService;
        private readonly IFileSystemService _fileSystemService;
        private readonly IFileBrowserService _fileBrowserService;
        #endregion

        #region Constructors
        public NavigatorConfigurationSynchronizer(IConfigurationService configurationService, IFileBrowserConfigurationService fileBrowserConfigurationService, IFileSystemService fileSystemService,
            IFileBrowserService fileBrowserService)
        {
            Argument.IsNotNull(() => configurationService);
            Argument.IsNotNull(() => fileSystemService);
            Argument.IsNotNull(() => fileBrowserConfigurationService);
            Argument.IsNotNull(() => fileBrowserService);

            _fileBrowserConfigurationService = fileBrowserConfigurationService;
            _fileSystemService = fileSystemService;
            _fileBrowserService = fileBrowserService;

            configurationService.ConfigurationChanged += OnConfigurationChanged;

            ApplyConfiguration();
        }
        #endregion

        #region Methods
        private void OnConfigurationChanged(object sender, ConfigurationChangedEventArgs e)
        {
            ApplyConfiguration();
        }

        private void ApplyConfiguration()
        {
            var foldersFromConfig = _fileBrowserConfigurationService.LoadFolders().ToArray();
            var fileBrowserModel = _fileBrowserService.FileBrowserModel;

            var folderNodes = fileBrowserModel.Directories;
            var foldersFromNavigator = folderNodes.Select(x => x.FullName).ToArray();

            UpdateFolderNodes(foldersFromConfig, foldersFromNavigator, folderNodes);
        }

        private void UpdateFolderNodes(string[] foldersFromConfig, string[] foldersFromNavigator, ObservableCollection<FolderNode> directories)
        {
            Argument.IsNotNull(() => foldersFromConfig);
            Argument.IsNotNull(() => foldersFromNavigator);
            Argument.IsNotNull(() => directories);

            var newFolderNodes = foldersFromConfig.Except(foldersFromNavigator).Select(folder => _fileSystemService.LoadFileSystemContent(folder, true));
            var foldersToRemove = foldersFromNavigator.Except(foldersFromConfig);

            foreach (var folder in foldersToRemove)
            {
                var folderNode = directories.FirstOrDefault(x => string.Equals(x.FullName.ToLower(), folder.ToLower()));
                directories.Remove(folderNode);
            }

            foreach (var folderNode in newFolderNodes)
            {
                directories.Add(folderNode);
            }
        }
        #endregion
    }
}