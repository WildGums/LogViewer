// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileExitCommandContainer.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace LogViewer
{
    using Catel;
    using Catel.MVVM;
    using Catel.Services;

    public class FileExitCommandContainer : CommandContainerBase
    {
        private readonly INavigationService _navigationService;

        public FileExitCommandContainer(ICommandManager commandManager, INavigationService navigationService)
            : base(Commands.File.Exit, commandManager)
        {
            Argument.IsNotNull(() => navigationService);

            _navigationService = navigationService;
        }

        protected override void Execute(object parameter)
        {
            _navigationService.CloseApplication();
        }
    }
}
