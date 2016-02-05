// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileExitCommandContainer.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
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
