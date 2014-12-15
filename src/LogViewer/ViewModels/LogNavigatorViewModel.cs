// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogNavigatorViewModel.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace LogViewer.ViewModels
{
    using Behaviors;
    using Catel.Fody;
    using Catel.MVVM;
    using Catel.Services;
    using Extensions;
    using Factories;
    using Models;
    using Models.Base;
    using Orchestra.Services;

    public class LogNavigatorViewModel : ViewModelBase, IHasSelectableItems
    {
        private readonly IAppDataService _appDataService;
        private readonly ICompanyFactory _companyFactory;
        private readonly IMessageService _messageService;
        private readonly ISelectDirectoryService _selectDirectoryService;

        public LogNavigatorViewModel(LogViewerModel logViewerModel, ISelectDirectoryService selectDirectoryService, IMessageService messageService, ICompanyFactory companyFactory, IAppDataService appDataService)
        {
            _selectDirectoryService = selectDirectoryService;
            _messageService = messageService;
            _companyFactory = companyFactory;
            _appDataService = appDataService;

            LogViewer = logViewerModel;

            AddCompanyCommand = new Command(OnAddCompanyCommandExecute);
        }

        [Model]
        [Expose("Companies")]
        public LogViewerModel LogViewer { get; set; }

        /// <summary>
        /// Gets the AddCompanyCommand command.
        /// </summary>
        public Command AddCompanyCommand { get; private set; }

        [ViewModelToModel("LogViewer")]
        public TreeNode SelectedItem { get; set; }

        /// <summary>
        /// Method to invoke when the AddCompanyCommand command is executed.
        /// </summary>
        private void OnAddCompanyCommandExecute()
        {
            var rootAppDataDir = _appDataService.GetRootAppDataFolder();

            _selectDirectoryService.Title = "Select Company's application data folder";
            _selectDirectoryService.InitialDirectory = rootAppDataDir;
            if (_selectDirectoryService.DetermineDirectory())
            {
                var companyFolder = _selectDirectoryService.DirectoryName;
                if (string.Equals(companyFolder, rootAppDataDir) || !companyFolder.StartsWith(rootAppDataDir))
                {
                    _messageService.ShowError(string.Format("Must be selected subdirectory of the\"{0}\"", rootAppDataDir));
                    return;
                }

                var company = _companyFactory.CreateNewCompanyItem(companyFolder);
                if (company != null)
                {
                    LogViewer.Companies.Add(company);
                }
            }
        }
    }
}