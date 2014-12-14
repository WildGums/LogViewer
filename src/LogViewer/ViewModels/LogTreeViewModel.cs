// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogTreeViewModel.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace LogViewer.ViewModels
{
    using System;
    using System.Windows.Forms;

    using Catel.Fody;
    using Catel.IO;
    using Catel.Logging;
    using Catel.MVVM;
    using Catel.Services;
    using Extensions;
    using Factories;
    using Models;
    using Orchestra.Services;
    using Views;

    public class LogTreeViewModel : ViewModelBase
    {
        private readonly ISelectDirectoryService _selectDirectoryService;
        private readonly IMessageService _messageService;
        private readonly ICompanyFactory _companyFactory;
        private readonly IAppDataService _appDataService;

        public LogTreeViewModel(LogViewerModel logViewerModel, ISelectDirectoryService selectDirectoryService, IMessageService messageService, ICompanyFactory companyFactory, IAppDataService appDataService)
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