﻿namespace LogViewer
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Catel.Logging;
    using Catel.MVVM;
    using Catel.Reflection;
    using Catel.Services;

    public class SettingsGeneralCommandContainer: CommandContainerBase
    {
        private const string ViewModelType = "SettingsViewModel";

        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private readonly IUIVisualizerService _uiVisualizerService;
        private readonly IViewModelFactory _viewModelFactory;

        public SettingsGeneralCommandContainer(ICommandManager commandManager, IUIVisualizerService uiVisualizerService, IViewModelFactory viewModelFactory)
            : base(Commands.Settings.General, commandManager)
        {
            ArgumentNullException.ThrowIfNull(uiVisualizerService);
            ArgumentNullException.ThrowIfNull(viewModelFactory);

            _uiVisualizerService = uiVisualizerService;
            _viewModelFactory = viewModelFactory;
        }

        public override async Task ExecuteAsync(object parameter)
        {
            base.Execute(parameter);

            var settingsViewModelType = TypeCache.GetTypes(x => string.Equals(x.Name, ViewModelType)).FirstOrDefault();
            if (settingsViewModelType is null)
            {
                throw Log.ErrorAndCreateException<InvalidOperationException>("Cannot find type '{0}'", ViewModelType);
            }

            var viewModel = _viewModelFactory.CreateViewModel(settingsViewModelType, null, null);

            await _uiVisualizerService.ShowDialogAsync(viewModel);
        }
    }
}
