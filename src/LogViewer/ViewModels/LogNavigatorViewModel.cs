// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogNavigatorViewModel.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2014 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace LogViewer.ViewModels
{
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using Catel;
    using Catel.Collections;
    using Catel.Fody;
    using Catel.MVVM;
    using Catel.Services;

    using LogViewer.Models;
    using LogViewer.Services;

    using Orchestra.Services;

    public class LogNavigatorViewModel : ViewModelBase
    {
        #region Fields
        private readonly IAppDataService _appDataService;
        private readonly IFileBrowserService _fileBrowserService;
        private readonly IFileBrowserConfigurationService _fileBrowserConfigurationService;
        private readonly IMessageService _messageService;
        private readonly ISelectDirectoryService _selectDirectoryService;

        private ObservableCollection<NavigationNode> _prevSelectedItems;
        #endregion

        #region Constructors
        public LogNavigatorViewModel(FileBrowserModel fileBrowserModel, ISelectDirectoryService selectDirectoryService, IMessageService messageService, IAppDataService appDataService,
            IFileBrowserService fileBrowserService, IFileBrowserConfigurationService fileBrowserConfigurationService)
        {
            Argument.IsNotNull(() => fileBrowserModel);
            Argument.IsNotNull(() => selectDirectoryService);
            Argument.IsNotNull(() => messageService);
            Argument.IsNotNull(() => appDataService);
            Argument.IsNotNull(() => fileBrowserConfigurationService);

            _selectDirectoryService = selectDirectoryService;
            _messageService = messageService;
            _appDataService = appDataService;
            _fileBrowserService = fileBrowserService;
            _fileBrowserConfigurationService = fileBrowserConfigurationService;

            FileBrowser = fileBrowserModel;

            AddCompany = new Command(OnAddCompanyCommandExecute);
            DeleteCompany = new Command(OnDeleteCompanyCommandExecute, CanExecuteDeleteCompanyCommand);

            SelectedItems = new ObservableCollection<NavigationNode>();
        }
        #endregion

        #region Properties
        [Model]
        [Expose("Directories")]
        public FileBrowserModel FileBrowser { get; set; }

        [ViewModelToModel("FileBrowser")]
        public ObservableCollection<NavigationNode> SelectedItems { get; set; }
        #endregion

        #region Commands
        public Command AddCompany { get; private set; }

        private async void OnAddCompanyCommandExecute()
        {
            var rootAppDataDir = _appDataService.GetRootAppDataFolder();

            _selectDirectoryService.Title = "Select FolderNode's application data folder";
            _selectDirectoryService.InitialDirectory = rootAppDataDir;

            if (_selectDirectoryService.DetermineDirectory())
            {
                var folder = _selectDirectoryService.DirectoryName;

                if (FileBrowser.Directories.Any(x => string.Equals(x.FullName, folder)))
                {
                    await _messageService.ShowError(string.Format("The directory {0} is already added", folder));
                    return;
                }

                _fileBrowserConfigurationService.AddFolder(folder);
            }
        }

        public Command DeleteCompany { get; private set; }

        private void OnDeleteCompanyCommandExecute()
        {
            var selectedCompany = SelectedItems.SingleOrDefault() as FolderNode;
            if (selectedCompany != null)
            {
                FileBrowser.Directories.Remove(selectedCompany);
            }
        }

        private bool CanExecuteDeleteCompanyCommand()
        {
            if (SelectedItems.Count != 1)
            {
                return false;
            }

            var selectedCompany = SelectedItems.SingleOrDefault() as FolderNode;
            return selectedCompany != null;
        }
        #endregion

        #region Methods
        public void OnSelectedItemsChanged()
        {
            if (_prevSelectedItems != null)
            {
                _prevSelectedItems.CollectionChanged -= OnSelectedItemsCollectionChanged;
            }

            if (SelectedItems != null)
            {
                SelectedItems.CollectionChanged += OnSelectedItemsCollectionChanged;
            }

            _prevSelectedItems = SelectedItems;
        }

        private void OnSelectedItemsCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            DeleteCompany.RaiseCanExecuteChanged();
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