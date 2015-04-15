// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogNavigatorViewModel.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace LogViewer.ViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using Catel;
    using Catel.Fody;
    using Catel.MVVM;
    using Catel.Services;
    using Models;
    using Orchestra.Services;
    using Services;

    public class LogNavigatorViewModel : ViewModelBase
    {
        #region Fields
        private readonly IAppDataService _appDataService;
        private readonly IFileBrowserConfigurationService _fileBrowserConfigurationService;
        private readonly IFileNodeService _fileNodeService;
        private readonly IFileBrowserService _fileBrowserService;
        private readonly IMessageService _messageService;
        private readonly ISelectDirectoryService _selectDirectoryService;
        private ObservableCollection<NavigationNode> _prevSelectedItems;
        #endregion

        #region Constructors
        public LogNavigatorViewModel(ISelectDirectoryService selectDirectoryService, IMessageService messageService, IAppDataService appDataService,
            IFileBrowserService fileBrowserService, IFileBrowserConfigurationService fileBrowserConfigurationService, IFileNodeService fileNodeService)
        {
            Argument.IsNotNull(() => selectDirectoryService);
            Argument.IsNotNull(() => messageService);
            Argument.IsNotNull(() => appDataService);
            Argument.IsNotNull(() => fileBrowserConfigurationService);
            Argument.IsNotNull(() => fileNodeService);

            _selectDirectoryService = selectDirectoryService;
            _messageService = messageService;
            _appDataService = appDataService;
            _fileBrowserService = fileBrowserService;
            _fileBrowserConfigurationService = fileBrowserConfigurationService;
            _fileNodeService = fileNodeService;

            FileBrowser = _fileBrowserService.FileBrowserModel;

            AddFolder = new Command(OnAddFolderExecute);
            DeleteFolder = new Command(OnDeleteFolderExecute, OnDeleteFolderCanExecute);
        }
        #endregion

        #region Properties
        [Model(SupportIEditableObject = false)]
        [Expose("RootDirectories")]
        [Expose("SelectedItems")]
        public FileBrowserModel FileBrowser { get; set; }
        #endregion

        #region Commands
        public Command AddFolder { get; private set; }

        private async void OnAddFolderExecute()
        {
            var rootAppDataDir = _appDataService.GetRootAppDataFolder();

            _selectDirectoryService.Title = "Select FolderNode's application data folder";
            _selectDirectoryService.InitialDirectory = rootAppDataDir;

            if (_selectDirectoryService.DetermineDirectory())
            {
                var folder = _selectDirectoryService.DirectoryName;

                if (FileBrowser.RootDirectories.Any(x => string.Equals(x.FullName, folder)))
                {
                    await _messageService.ShowError(string.Format("The directory {0} is already added", folder));
                    return;
                }

                _fileBrowserConfigurationService.AddFolder(folder);
            }
        }

        public Command DeleteFolder { get; private set; }

        private void OnDeleteFolderExecute()
        {
            var folder = FileBrowser.SelectedItems.SingleOrDefault() as FolderNode;
            if (folder != null)
            {
                _fileBrowserConfigurationService.RemoveFolder(folder.FullName);
            }
        }

        private bool OnDeleteFolderCanExecute()
        {
            if (FileBrowser.SelectedItems.Count != 1)
            {
                return false;
            }

            var folderNode = FileBrowser.SelectedItems.SingleOrDefault() as FolderNode;
            if (folderNode == null)
            {
                return false;
            }

            return FileBrowser.RootDirectories.Contains(folderNode);
        }
        #endregion

        #region Methods
        public void OnSelectedItemsChanged()
        {
            if (_prevSelectedItems != null)
            {
                _prevSelectedItems.CollectionChanged -= OnSelectedItemsCollectionChanged;
            }

            if (FileBrowser.SelectedItems != null)
            {
                FileBrowser.SelectedItems.CollectionChanged += OnSelectedItemsCollectionChanged;
            }

            _prevSelectedItems = FileBrowser.SelectedItems;
        }

        private void OnSelectedItemsCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            DeleteFolder.RaiseCanExecuteChanged();
            var fileNodes = FileBrowser.SelectedItems.OfType<FileNode>().ToArray();

            foreach (var fileNode in fileNodes)
            {
                _fileNodeService.LoadFileNode(fileNode);
            }
        }

        protected override async Task Initialize()
        {
            await base.Initialize();
        }

        protected override async Task<bool> Save()
        {
            return await base.Save();
        }
        #endregion
    }
}