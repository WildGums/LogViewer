// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ApplicationInitializationService.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2014 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace LogViewer.Services
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Media;
    using Catel;
    using Catel.Data;
    using Catel.IoC;
    using Catel.Logging;
    using Catel.MVVM;
    using Configuration;
    using MethodTimer;
    using Models;
    using Orc.Analytics;
    using Orc.Squirrel;
    using Orc.WorkspaceManagement;
    using Orchestra.Markup;
    using Orchestra.Services;
    using Orchestra.Shell.Services;
    using Settings = LogViewer.Settings;
    using Catel.Windows.Controls;
    using Orc.FilterBuilder.Services;

    public class ApplicationInitializationService : ApplicationInitializationServiceBase
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private readonly ITypeFactory _typeFactory;
        private readonly IServiceLocator _serviceLocator;

        public ApplicationInitializationService(ITypeFactory typeFactory, IServiceLocator serviceLocator)
        {
            Argument.IsNotNull(() => typeFactory);
            Argument.IsNotNull(() => serviceLocator);

            _typeFactory = typeFactory;
            _serviceLocator = serviceLocator;
        }

        #region Methods
        public override async Task InitializeBeforeCreatingShell()
        {
            // Non-async first
            await RegisterTypes();
            await InitializeFonts();
            await InitializeSettings();

            await RunAndWaitAsync(new Func<Task>[]
            {
                ImprovePerformance,
                InitializeAnalytics,
                //InitializeAutomaticSupport,
                //InitializeFeedback,
                InitializeFilters,
                InitializeWorkspaces,
                CheckForUpdates
            });
        }

        public override async Task InitializeCommands(ICommandManager commandManager)
        {
            Argument.IsNotNull(() => commandManager);

            commandManager.CreateCommand(Commands.File.Exit, throwExceptionWhenCommandIsAlreadyCreated: false);

            commandManager.CreateCommand(Commands.Filter.ResetSearchTemplate, throwExceptionWhenCommandIsAlreadyCreated: false);

            commandManager.CreateCommand(Commands.Settings.General, throwExceptionWhenCommandIsAlreadyCreated: false);

            commandManager.CreateCommand(Commands.Help.About, throwExceptionWhenCommandIsAlreadyCreated: false);

            await base.InitializeCommands(commandManager);
        }

        private async Task RegisterTypes()
        {
            var serviceLocator = ServiceLocator.Default;

            serviceLocator.RegisterType<IManageUserDataService, ManageUserDataService>();

            serviceLocator.RegisterType<IConfigurationInitializationService, ConfigurationInitializationService>();
            serviceLocator.RegisterType<IFilterCustomizationService, FilterCustomizationService>();

            serviceLocator.RegisterType<ILogRecordService, LogRecordService>();
            serviceLocator.RegisterType<ILogFileService, LogFileService>();
            serviceLocator.RegisterType<IFilterService, FilterService>();
            serviceLocator.RegisterType<IRegexService, RegexService>();
            serviceLocator.RegisterType<IFileBrowserConfigurationService, FileBrowserConfigurationService>();
            serviceLocator.RegisterType<IFileSystemService, FileSystemService>();
            serviceLocator.RegisterType<IFileBrowserService, FileBrowserService>();
            serviceLocator.RegisterType<IIndexSearchService, IndexSearchService>();

            serviceLocator.RegisterTypeAndInstantiate<FileBrowserModel>();
        }

        [Time]
        private async Task ImprovePerformance()
        {
            Log.Info("Improving performance");

            ModelBase.DefaultSuspendValidationValue = true;
            UserControl.DefaultCreateWarningAndErrorValidatorForViewModelValue = false;
            UserControl.DefaultSkipSearchingForInfoBarMessageControlValue = true;
        }

        [Time]
        private async Task InitializeAnalytics()
        {
            Log.Info("Initializing analytics");

            var analyticsConfigurationSynchronizer = _typeFactory.CreateInstance<AnalyticsConfigurationSynchronizer>();
            _serviceLocator.RegisterInstance(analyticsConfigurationSynchronizer);

            var googleAnalyticsService = _serviceLocator.ResolveType<IGoogleAnalyticsService>();
            googleAnalyticsService.AccountId = Analytics.AccountId;

            _serviceLocator.RegisterTypeAndInstantiate<NavigatorConfigurationSynchronizer>();
        }

        [Time]
        private async Task InitializeFonts()
        {
            FontImage.RegisterFont("FontAwesome", new FontFamily(new Uri("pack://application:,,,/LogViewer;component/Resources/Fonts/", UriKind.RelativeOrAbsolute), "./#FontAwesome"));

            FontImage.DefaultFontFamily = "FontAwesome";

            FontImage.DefaultBrush = new SolidColorBrush(Color.FromArgb(255, 87, 87, 87));
        }

        [Time]
        private async Task InitializeSettings()
        {
            Log.Info("Initializing settings");

            var configurationInitializationService = _serviceLocator.ResolveType<IConfigurationInitializationService>();
            configurationInitializationService.Initialize();
        }

        [Time]
        private async Task CheckForUpdates()
        {
            Log.Info("Checking for updates");

            var maximumReleaseDate = DateTime.MaxValue;

            var updateService = _serviceLocator.ResolveType<IUpdateService>();
            updateService.Initialize(Settings.Application.AutomaticUpdates.AvailableChannels, Settings.Application.AutomaticUpdates.DefaultChannel,
                Settings.Application.AutomaticUpdates.CheckForUpdatesDefaultValue);

#pragma warning disable 4014
            // Not dot await, it's a background thread
            updateService.HandleUpdates(maximumReleaseDate);
#pragma warning restore 4014
        }

        [Time]
        private async Task InitializeFilters()
        {
            Log.Info("Initializing filters");

            var filterSchemeManager = _serviceLocator.ResolveType<IFilterSchemeManager>();
            filterSchemeManager.Load();
        }

        [Time]
        private async Task InitializeWorkspaces()
        {
            Log.Info("Initializing workspaces");

            var workspaceManager = _serviceLocator.ResolveType<IWorkspaceManager>();
            await workspaceManager.Initialize(defaultWorkspaceName: Workspaces.DefaultWorkspaceName);

            var defaultWorkspace = (from workspace in workspaceManager.Workspaces
                                    where string.Equals(workspace.Title, Workspaces.DefaultWorkspaceName)
                                    select workspace).FirstOrDefault();
            if (defaultWorkspace != null)
            {
                defaultWorkspace.CanDelete = false;
                defaultWorkspace.CanEdit = false;
            }
        }
        #endregion
    }
}