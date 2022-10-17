namespace LogViewer
{
    using System;
    using System.Threading.Tasks;
    using Catel.MVVM;
    using Catel.Services;

    public class FileExitCommandContainer : CommandContainerBase
    {
        private readonly INavigationService _navigationService;

        public FileExitCommandContainer(ICommandManager commandManager, INavigationService navigationService)
            : base(Commands.File.Exit, commandManager)
        {
            ArgumentNullException.ThrowIfNull(navigationService);

            _navigationService = navigationService;
        }

        protected override async Task ExecuteAsync(object parameter)
        {
            await _navigationService.CloseApplicationAsync();
        }
    }
}
