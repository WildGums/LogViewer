// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogNavigatorViewModel.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace LogViewer.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    using Behaviors;

    using Catel.Collections;
    using Catel.Configuration;
    using Catel.Fody;
    using Catel.MVVM;
    using Catel.Services;
    using Extensions;
    using Models;
    using Models.Base;
    using Orchestra.Services;
    using Services;

    public class LogNavigatorViewModel : ViewModelBase, IHasSelectableItems
    {
        private readonly IAppDataService _appDataService;

        private readonly ICompanyService _companyService;
        private readonly IMessageService _messageService;
        private readonly ISelectDirectoryService _selectDirectoryService;

        public LogNavigatorViewModel(LogViewerModel logViewerModel, ISelectDirectoryService selectDirectoryService, IMessageService messageService, ICompanyService companyService, IAppDataService appDataService)
        {
            _selectDirectoryService = selectDirectoryService;
            _messageService = messageService;
            _companyService = companyService;
            _appDataService = appDataService;
            
            LogViewer = logViewerModel;

            AddCompanyCommand = new Command(OnAddCompanyCommandExecute);

            
        }

        protected override async Task Initialize()
        {
            if (LogViewer.Companies == null)
            {
                LogViewer.Companies = new ObservableCollection<Company>();
            }
            else
            {
                LogViewer.Companies.Clear();
            }
            LogViewer.Companies.AddRange(_companyService.LoadCompanies());
        }

        protected override void OnClosing()
        {
            _companyService.SaveCompanies(LogViewer.Companies);
            base.OnClosing();
        }

        [Model]
        [Expose("Companies")]
        public LogViewerModel LogViewer { get; set; }

        /// <summary>
        /// Gets the AddCompanyCommand command.
        /// </summary>
        public Command AddCompanyCommand { get; private set; }

        [ViewModelToModel("LogViewer")]
        public NavigationNode SelectedItem { get; set; }

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

                var companyName = new DirectoryInfo(companyFolder).Name;
                if (LogViewer.Companies.Any(x => string.Equals(x.Name, companyName)))
                {
                    _messageService.ShowError(string.Format("The company {0} is already added", companyName));
                    return;
                }

                var company = _companyService.CreateCompanyByName(companyName);
                if (company != null)
                {
                    LogViewer.Companies.Add(company);
                }
            }
        }
    }
}