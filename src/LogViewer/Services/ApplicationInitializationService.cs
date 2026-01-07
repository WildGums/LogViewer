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
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Models;
    using Orc.FilterBuilder;
    using Orc.Squirrel;
    using Orc.WorkspaceManagement;
    using Orchestra;
    using Settings = LogViewer.Settings;

    public class ApplicationInitializationService : ApplicationInitializationServiceBase
    {
        private static readonly ILogger Logger = LogManager.GetLogger(typeof(ApplicationInitializationService));

        private readonly ICommandManager _commandManager;

        public ApplicationInitializationService(IServiceProvider serviceProvider, ICommandManager commandManager)
            : base(serviceProvider)
        {
            _commandManager = commandManager;
        }

        public override async Task InitializeBeforeCreatingShellAsync()
        {
            // Non-async first
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
            Logger.LogInformation("Initializing commands");

            _commandManager.CreateCommandWithGesture(ServiceProvider, typeof(Commands.File), nameof(Commands.File.Exit));

            _commandManager.CreateCommandWithGesture(ServiceProvider, typeof(Commands.Filter), nameof(Commands.Filter.ResetSearchTemplate));
            _commandManager.CreateCommandWithGesture(ServiceProvider, typeof(Commands.Filter), nameof(Commands.Filter.ExportResult));
            _commandManager.CreateCommandWithGesture(ServiceProvider, typeof(Commands.Filter), nameof(Commands.Filter.CopyResultToClipboard));

            _commandManager.CreateCommandWithGesture(ServiceProvider, typeof(Commands.Settings), nameof(Commands.Settings.General));

            _commandManager.CreateCommandWithGesture(ServiceProvider, typeof(Commands.Help), nameof(Commands.Help.About));
        }

        [Time]
        private async Task ImprovePerformanceAsync()
        {
            Logger.LogInformation("Improving performance");

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
            Logger.LogInformation("Initializing settings");

            var configurationInitializationService = ServiceProvider.GetRequiredService<IConfigurationInitializationService>();
            configurationInitializationService.Initialize();
        }

        [Time]
        private async Task CheckForUpdatesAsync()
        {
            Logger.LogInformation("Checking for updates");

            var updateService = ServiceProvider.GetRequiredService<IUpdateService>();
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
            Logger.LogInformation("Initializing filters");

            var filterSchemeManager = ServiceProvider.GetRequiredService<IFilterSchemeManager>();
            await filterSchemeManager.LoadAsync();
        }

        [Time]
        private async Task InitializeWorkspacesAsync()
        {
            Logger.LogInformation("Initializing workspaces");

            var workspaceManager = ServiceProvider.GetRequiredService<IWorkspaceManager>();

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
