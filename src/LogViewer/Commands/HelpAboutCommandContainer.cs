namespace LogViewer
{
    using System;
    using System.Threading.Tasks;
    using Catel;
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

        protected override Task ExecuteAsync(object parameter)
        {
            return _aboutService.ShowAboutAsync();
        }
    }
}
