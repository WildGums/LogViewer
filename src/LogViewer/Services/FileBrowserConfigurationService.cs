// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileBrowserConfigurationService.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace LogViewer.Services
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Catel;
    using Catel.Configuration;

    public class FileBrowserConfigurationService : IFileBrowserConfigurationService
    {
        #region Fields
        private const string BrowserRootsKey = "BrowserRoots";
        private const string RootsSeparator = "|";
        private readonly IConfigurationService _configurationService;
        #endregion

        #region Constructors
        public FileBrowserConfigurationService(IConfigurationService configurationService)
        {
            Argument.IsNotNull(() => configurationService);

            _configurationService = configurationService;
        }
        #endregion

        #region Methods
        public bool AddFolder(string fullPath)
        {
            Argument.IsNotNullOrWhitespace(() => fullPath);

            if (!Directory.Exists(fullPath))
            {
                return false;
            }

            var folders = LoadFolders().ToList();
            if (folders.Any(x => string.Equals(x.ToLower(), fullPath.ToLower())))
            {
                return false;
            }

            folders.Add(fullPath);
            SaveFolders(folders);

            return true;
        }

        public void RemoveFolder(string fullPath)
        {
            Argument.IsNotNullOrWhitespace(() => fullPath);

            var folders = LoadFolders();
            var result = folders.Where(folder => !string.Equals(folder.ToLower(), fullPath.ToLower()));
            SaveFolders(result);
        }

        public IEnumerable<string> LoadFolders()
        {
            var value = _configurationService.GetRoamingValue(BrowserRootsKey, string.Empty);

            string[] folders;
            if (string.IsNullOrEmpty(value))
            {
                folders = new string[] { };
            }
            else
            {
                folders = value.Split(new[] { RootsSeparator }, StringSplitOptions.None);
            }

            var existedFolders = folders.Where(Directory.Exists).ToArray();
            if (existedFolders.Length != folders.Length)
            {
                SaveFolders(existedFolders);
            }

            return existedFolders;
        }

        private void SaveFolders(IEnumerable<string> folders)
        {
            Argument.IsNotNull(() => folders);

            var foldersValue = string.Join(RootsSeparator, folders);
            _configurationService.SetRoamingValue(BrowserRootsKey, foldersValue);
        }
        #endregion
    }
}