namespace LogViewer
{
    using System;
    using System.Threading.Tasks;
    using Catel.MVVM;
    using Orchestra;

    public class HelpAboutCommandContainer: CommandContainerBase
    {
        private readonly IAboutService _aboutService;

        public HelpAboutCommandContainer(ICommandManager commandManager, IAboutService aboutService,
            IServiceProvider serviceProvider)
            : base(Commands.Help.About, commandManager, serviceProvider)
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
