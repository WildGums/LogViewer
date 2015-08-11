// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HelpAboutCommandContainer.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace LogViewer
{
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

        protected override void Execute(object parameter)
        {
            _aboutService.ShowAbout();
        }
    }
}