namespace LogViewer
{
    using System;
    using System.Threading.Tasks;
    using Catel.MVVM;
    using Orchestra.Services;

    public class HelpAboutCommandContainer: CommandContainerBase
    {
        private readonly IAboutService _aboutService;

        public HelpAboutCommandContainer(ICommandManager commandManager, IAboutService aboutService)
            : base(Commands.Help.About, commandManager)
        {
            ArgumentNullException.ThrowIfNull(aboutService);

            _aboutService = aboutService;
        }

        public override Task ExecuteAsync(object parameter)
        {
            return _aboutService.ShowAboutAsync();
        }
    }
}
