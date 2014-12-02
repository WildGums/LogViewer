using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Catel.MVVM;
using Orchestra.Services;
using InputGesture = Catel.Windows.Input.InputGesture;

namespace LogViewer.Services
{
    public class ApplicationInitializationService : ApplicationInitializationServiceBase
    {
        public override async Task InitializeBeforeCreatingShell()
        {
            await RunAndWaitAsync(new Func<Task>[]
            {
                InitializePerformance
            });
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
    }
}