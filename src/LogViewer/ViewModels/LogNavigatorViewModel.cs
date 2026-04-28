namespace LogViewer.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using Catel.Fody;
    using Catel.MVVM;
    using Catel.Services;
    using Models;
    using Services;

    public class LogNavigatorViewModel : FeaturedViewModelBase
    {
        private readonly IAppDataService _appDataService;
        private readonly IFileBrowserConfigurationService _fileBrowserConfigurationService;
        private readonly IFileNodeService _fileNodeService;
        private readonly IMessageService _messageService;
        private readonly ISelectDirectoryService _selectDirectoryService;
        private ObservableCollection<NavigationNode> _prevSelectedItems;

        public LogNavigatorViewModel(ISelectDirectoryService selectDirectoryService, IMessageService messageService, 
            IAppDataService appDataService, IFileBrowserService fileBrowserService, 
            IFileBrowserConfigurationService fileBrowserConfigurationService, IFileNodeService fileNodeService,
            IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
            _selectDirectoryService = selectDirectoryService;
            _messageService = messageService;
            _appDataService = appDataService;
            _fileBrowserConfigurationService = fileBrowserConfigurationService;
            _fileNodeService = fileNodeService;

            FileBrowser = fileBrowserService.FileBrowserModel;

            AddFolder = new TaskCommand(serviceProvider, OnAddFolderExecuteAsync);
            DeleteFolder = new TaskCommand(serviceProvider, OnDeleteFolderExecuteAsync, OnDeleteFolderCanExecute);
        }

        [Model(SupportIEditableObject = false)]
        [Expose("RootDirectories")]
        [Expose("SelectedItems")]
        public FileBrowserModel FileBrowser { get; set; }

        public void OnSelectedItemsChanged()
        {
            if (_prevSelectedItems is not null)
            {
                _prevSelectedItems.CollectionChanged -= OnSelectedItemsCollectionChanged;
            }

            if (FileBrowser.SelectedItems is not null)
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

        public TaskCommand AddFolder { get; private set; }

        private async Task OnAddFolderExecuteAsync()
        {
            var rootAppDataDir = _appDataService.GetRootAppDataFolder();

            var result = await _selectDirectoryService.DetermineDirectoryAsync(new DetermineDirectoryContext
            {
                Title = "Select FolderNode's application data folder",
                InitialDirectory = rootAppDataDir
            });

            if (result.Result)
            {
                var folder = result.DirectoryName;

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
            if (folder is not null)
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
            if (folderNode is null)
            {
                return false;
            }

            return FileBrowser.RootDirectories.Contains(folderNode);
        }
    }
}
