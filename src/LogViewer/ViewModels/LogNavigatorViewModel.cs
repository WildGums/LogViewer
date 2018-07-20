// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogNavigatorViewModel.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace LogViewer.ViewModels
{
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
            _fileBrowserConfigurationService = fileBrowserConfigurationService;
            _fileNodeService = fileNodeService;

            FileBrowser = fileBrowserService.FileBrowserModel;

            AddFolder = new TaskCommand(OnAddFolderExecuteAsync);
            DeleteFolder = new TaskCommand(OnDeleteFolderExecuteAsync, OnDeleteFolderCanExecute);
        }
        #endregion

        #region Properties
        [Model(SupportIEditableObject = false)]
        [Expose("RootDirectories")]
        [Expose("SelectedItems")]
        public FileBrowserModel FileBrowser { get; set; }
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

            _fileNodeService.ParallelLoadFileNodeBatch(fileNodes);
        }
        #endregion

        #region Commands
        public TaskCommand AddFolder { get; private set; }

        private async Task OnAddFolderExecuteAsync()
        {
            var rootAppDataDir = _appDataService.GetRootAppDataFolder();

            _selectDirectoryService.Title = "Select FolderNode's application data folder";
            _selectDirectoryService.InitialDirectory = rootAppDataDir;

            if (await _selectDirectoryService.DetermineDirectoryAsync())
            {
                var folder = _selectDirectoryService.DirectoryName;

                if (FileBrowser.RootDirectories.Any(x => string.Equals(x.FullName, folder)))
                {
                    await _messageService.ShowErrorAsync(string.Format("The directory {0} is already added", folder));
                    return;
                }

                _fileBrowserConfigurationService.AddFolder(folder);
            }
        }

        public TaskCommand DeleteFolder { get; private set; }

        private async Task OnDeleteFolderExecuteAsync()
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
    }
}