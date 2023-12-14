namespace LogViewer.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Media;
    using Catel;
    using Catel.IoC;
    using Catel.Logging;
    using Catel.MVVM;
    using Catel.Windows.Controls;
    using Configuration;
    using Fluent;
    using LogViewer.Views;
    using MethodTimer;
    using Models;
    using Orc.FilterBuilder;
    using Orc.Squirrel;
    using Orc.WorkspaceManagement;
    using Orchestra.Services;
    using Settings = LogViewer.Settings;

    public class ApplicationInitializationService : ApplicationInitializationServiceBase
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();
        private readonly IServiceLocator _serviceLocator;
        private readonly ICommandManager _commandManager;
        private readonly ITypeFactory _typeFactory;

        public ApplicationInitializationService(ITypeFactory typeFactory, IServiceLocator serviceLocator, ICommandManager commandManager)
        {
            ArgumentNullException.ThrowIfNull(typeFactory);
            ArgumentNullException.ThrowIfNull(serviceLocator);
            ArgumentNullException.ThrowIfNull(commandManager);

            _typeFactory = typeFactory;
            _serviceLocator = serviceLocator;
            _commandManager = commandManager;
        }

        public override async Task InitializeBeforeCreatingShellAsync()
        {
            // Non-async first
            RegisterTypes();
            InitializeFonts();
            InitializeSettings();
            InitializeCommands();

            var tasks = new List<Task>()
            {
                Task.Run(ImprovePerformanceAsync),
                Task.Run(InitializeFiltersAsync),
                Task.Run(InitializeWorkspacesAsync),
                Task.Run(CheckForUpdatesAsync)
            };

            await Task.WhenAll(tasks);
        }

        public override async Task InitializeAfterCreatingShellAsync()
        {
            var shellWindow = System.Windows.Application.Current.MainWindow as RibbonWindow;

            var windowCommands = new WindowCommands();
            windowCommands.Items.Add(new WindowCommandsView());
            shellWindow.WindowCommands = windowCommands;

            await base.InitializeAfterCreatingShellAsync();
        }

        private void InitializeCommands()
        {
            Log.Info("Initializing commands");

            _commandManager.CreateCommandWithGesture(typeof(Commands.File), nameof(Commands.File.Exit));

            _commandManager.CreateCommandWithGesture(typeof(Commands.Filter), nameof(Commands.Filter.ResetSearchTemplate));
            _commandManager.CreateCommandWithGesture(typeof(Commands.Filter), nameof(Commands.Filter.ExportResult));
            _commandManager.CreateCommandWithGesture(typeof(Commands.Filter), nameof(Commands.Filter.CopyResultToClipboard));

            _commandManager.CreateCommandWithGesture(typeof(Commands.Settings), nameof(Commands.Settings.General));

            _commandManager.CreateCommandWithGesture(typeof(Commands.Help), nameof(Commands.Help.About));
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
        private void InitializeFonts()
        {
            Orc.Theming.FontImage.RegisterFont("FontAwesome", new FontFamily(new Uri("pack://application:,,,/LogViewer;component/Resources/Fonts/", UriKind.RelativeOrAbsolute), "./#FontAwesome"));
            Orc.Theming.FontImage.DefaultFontFamily = "FontAwesome";
            Orc.Theming.FontImage.DefaultBrush = new SolidColorBrush(Color.FromArgb(255, 87, 87, 87));
        }

        [Time]
        private void InitializeSettings()
        {
            Log.Info("Initializing settings");

            _serviceLocator.RegisterTypeAndInstantiate<NavigatorConfigurationSynchronizer>();
            _serviceLocator.RegisterTypeAndInstantiate<TimestampVisibilityConfigurationSynchronizer>();

            var configurationInitializationService = _serviceLocator.ResolveType<IConfigurationInitializationService>();
            configurationInitializationService.Initialize();
        }

        [Time]
        private async Task CheckForUpdatesAsync()
        {
            Log.Info("Checking for updates");

            var updateService = _serviceLocator.ResolveType<IUpdateService>();
            await updateService.InitializeAsync(Settings.Application.AutomaticUpdates.AvailableChannels, Settings.Application.AutomaticUpdates.DefaultChannel,
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
            if (defaultWorkspace is not null)
            {
                defaultWorkspace.CanDelete = false;
                defaultWorkspace.CanEdit = false;
            }
        }
    }
}
