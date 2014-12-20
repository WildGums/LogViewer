// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ApplicationInitializationService.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace LogViewer.Services
{
    using System;
    using System.Threading.Tasks;

    using Catel.MVVM;

    using Orchestra.Services;

    public class ApplicationInitializationService : ApplicationInitializationServiceBase
    {
        #region Methods
        public override Task InitializeCommands(ICommandManager commandManager)
        {
            commandManager.CreateCommand("Filter.ResetSearchTemplate", throwExceptionWhenCommandIsAlreadyCreated: false);
            return base.InitializeCommands(commandManager);
        }

        public override async Task InitializeBeforeCreatingShell()
        {
            await RunAndWaitAsync(new Func<Task>[] { InitializePerformance });
        }

        private async Task InitializePerformance()
        {
            Catel.Data.ModelBase.DefaultSuspendValidationValue = true;
            Catel.Windows.Controls.UserControl.DefaultCreateWarningAndErrorValidatorForViewModelValue = false;
            Catel.Windows.Controls.UserControl.DefaultSkipSearchingForInfoBarMessageControlValue = true;
        }
        #endregion
    }
}