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
        public override async Task InitializeBeforeCreatingShell()
        {
            await RunAndWaitAsync(new Func<Task>[] { InitializePerformance });
        }

        public override async Task InitializeCommands(ICommandManager commandManager)
        {
            // TODO: use commandManager.CreateCommand to create global command
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