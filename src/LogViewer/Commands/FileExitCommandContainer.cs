namespace LogViewer
{
    using System;
    using System.Threading.Tasks;
    using Catel.MVVM;
    using Catel.Services;

    public class FileExitCommandContainer : CommandContainerBase
    {
        private readonly INavigationService _navigationService;

        public FileExitCommandContainer(ICommandManager commandManager, INavigationService navigationService,
            IServiceProvider serviceProvider)
            : base(Commands.File.Exit, commandManager, serviceProvider)
        {
            ArgumentNullException.ThrowIfNull(navigationService);

            _navigationService = navigationService;
        }

        public override async Task ExecuteAsync(object parameter)
        {
            await _navigationService.CloseApplicationAsync();
        }
    }
}
