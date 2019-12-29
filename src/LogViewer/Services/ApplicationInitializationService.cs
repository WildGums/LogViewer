// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ApplicationInitializationService.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
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
    using Catel.Services;
    using Catel.Threading;
    using Catel.Windows.Controls;
    using Configuration;
    using MethodTimer;
    using Models;
    using Orc.Analytics;
    using Orc.FilterBuilder.Services;
    using Orc.Squirrel;
    using Orc.WorkspaceManagement;
    using Orchestra.Markup;
    using Orchestra.Services;
    using Settings = LogViewer.Settings;

    public class ApplicationInitializationService : ApplicationInitializationServiceBase
    {
        #region Fields
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();
        private readonly IServiceLocator _serviceLocator;
        private readonly ICommandManager _commandManager;
        private readonly ITypeFactory _typeFactory;
        #endregion

        #region Constructors
        public ApplicationInitializationService(ITypeFactory typeFactory, IServiceLocator serviceLocator, ICommandManager commandManager)
        {
            Argument.IsNotNull(() => typeFactory);
            Argument.IsNotNull(() => serviceLocator);
            Argument.IsNotNull(() => commandManager);

            _typeFactory = typeFactory;
            _serviceLocator = serviceLocator;
            _commandManager = commandManager;
        }
        #endregion

        #region Methods
        public override async Task InitializeBeforeCreatingShellAsync()
        {
            // Non-async first
            RegisterTypes();
            InitializeFonts();
            InitializeSettings();
            InitializeCommands();

            await TaskHelper.RunAndWaitAsync(new Func<Task>[] {
                ImprovePerformanceAsync,
                InitializeAnalyticsAsync,
                InitializeFiltersAsync,
                InitializeWorkspacesAsync,
                CheckForUpdatesAsync
            });
        }

        public override async Task InitializeAfterCreatingShellAsync()
        {
            await base.InitializeAfterCreatingShellAsync();
        }

        private void InitializeCommands()
        {
            Log.Info("Initializing commands");

            _commandManager.CreateCommandWithGesture(typeof(Commands.File), "Exit");

            _commandManager.CreateCommandWithGesture(typeof(Commands.Filter), "ResetSearchTemplate");
            _commandManager.CreateCommandWithGesture(typeof(Commands.Filter), "ExportResult");
            _commandManager.CreateCommandWithGesture(typeof(Commands.Filter), "CopyResultToClipboard");

            _commandManager.CreateCommandWithGesture(typeof(Commands.Settings), "General");

            _commandManager.CreateCommandWithGesture(typeof(Commands.Help), "About");
        }

        private void RegisterTypes()
        {
            var serviceLocator = ServiceLocator.Default;

            serviceLocator.RegisterType<IConfigurationInitializationService, ConfigurationInitializationService>();
            serviceLocator.RegisterType<IFilterCustomizationService, FilterCustomizationService>();

            serviceLocator.RegisterType<ILogReaderService, LogReaderService>();
            serviceLocator.RegisterType<IFileNodeService, FileNodeService>();
            serviceLocator.RegisterType<IFilterService, FilterService>();
            serviceLocator.RegisterType<IRegexService, RegexService>();
            serviceLocator.RegisterType<IFileBrowserConfigurationService, FileBrowserConfigurationService>();
            serviceLocator.RegisterType<IFileSystemService, FileSystemService>();
            serviceLocator.RegisterType<IFileBrowserService, FileBrowserService>();
            serviceLocator.RegisterType<IFileSystemWatchingService, FileSystemWatchingService>();
            serviceLocator.RegisterType<ILogTableService, LogTableService>();
            serviceLocator.RegisterType<INavigationNodeCacheService, NavigationNodeCacheService>();
            serviceLocator.RegisterType<ILogTableConfigurationService, LogTableConfigurationService>();

            serviceLocator.RegisterType<IWorkspaceInitializer, WorkspaceInitializer>();

            serviceLocator.RegisterTypeAndInstantiate<FileBrowserModel>();
            serviceLocator.RegisterTypeAndInstantiate<UnhandledExceptionWatcher>();
        }

        [Time]
        private async Task ImprovePerformanceAsync()
        {
            Log.Info("Improving performance");

            UserControl.DefaultCreateWarningAndErrorValidatorForViewModelValue = false;
            UserControl.DefaultSkipSearchingForInfoBarMessageControlValue = true;
        }

        [Time]
        private async Task InitializeAnalyticsAsync()
        {
            Log.Info("Initializing analytics");

            var analyticsConfigurationSynchronizer = _typeFactory.CreateInstance<AnalyticsConfigurationSynchronizer>();
            _serviceLocator.RegisterInstance(analyticsConfigurationSynchronizer);

            var analyticsService = _serviceLocator.ResolveType<IAnalyticsService>();
            analyticsService.AccountId = Analytics.AccountId;

            _serviceLocator.RegisterTypeAndInstantiate<NavigatorConfigurationSynchronizer>();
            _serviceLocator.RegisterTypeAndInstantiate<TimestampVisibilityConfigurationSynchronizer>();
        }

        [Time]
        private void InitializeFonts()
        {
            Orc.Controls.FontImage.RegisterFont("FontAwesome", new FontFamily(new Uri("pack://application:,,,/LogViewer;component/Resources/Fonts/", UriKind.RelativeOrAbsolute), "./#FontAwesome"));
            Orc.Controls.FontImage.DefaultFontFamily = "FontAwesome";
            Orc.Controls.FontImage.DefaultBrush = new SolidColorBrush(Color.FromArgb(255, 87, 87, 87));
        }

        [Time]
        private void InitializeSettings()
        {
            Log.Info("Initializing settings");

            var configurationInitializationService = _serviceLocator.ResolveType<IConfigurationInitializationService>();
            configurationInitializationService.Initialize();
        }

        [Time]
        private async Task CheckForUpdatesAsync()
        {
            Log.Info("Checking for updates");

            var updateService = _serviceLocator.ResolveType<IUpdateService>();
            updateService.Initialize(Settings.Application.AutomaticUpdates.AvailableChannels, Settings.Application.AutomaticUpdates.DefaultChannel,
                Settings.Application.AutomaticUpdates.CheckForUpdatesDefaultValue);

#pragma warning disable 4014
            // Not dot await, it's a background thread
            updateService.InstallAvailableUpdatesAsync(new SquirrelContext());
#pragma warning restore 4014
        }

        [Time]
        private async Task InitializeFiltersAsync()
        {
            Log.Info("Initializing filters");

            var filterSchemeManager = _serviceLocator.ResolveType<IFilterSchemeManager>();
            await filterSchemeManager.LoadAsync();
        }

        [Time]
        private async Task InitializeWorkspacesAsync()
        {
            Log.Info("Initializing workspaces");

            var workspaceManager = _serviceLocator.ResolveType<IWorkspaceManager>();

            await workspaceManager.AddProviderAsync<FilterWorkspaceProvider>(true);
            await workspaceManager.InitializeAsync(defaultWorkspaceName: Workspaces.DefaultWorkspaceName);

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
