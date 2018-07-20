// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HelpAboutCommandContainer.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace LogViewer
{
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
            Argument.IsNotNull(() => aboutService);

            _aboutService = aboutService;
        }

        protected override Task ExecuteAsync(object parameter)
        {
            return _aboutService.ShowAboutAsync();
        }
    }
}