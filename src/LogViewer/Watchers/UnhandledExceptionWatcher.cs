namespace LogViewer
{
    using System;
    using System.Diagnostics;
    using System.Threading.Tasks;
    using System.Windows;
    using Catel.IoC;
    using Catel.Logging;
    using Catel.Services;
    using Microsoft.Extensions.Logging;

    public class UnhandledExceptionWatcher : IConstructAtStartup
    {
        private static readonly ILogger Logger = LogManager.GetLogger(typeof(UnhandledExceptionWatcher));

        private readonly IMessageService _messageService;

        public UnhandledExceptionWatcher(IMessageService messageService)
        {
            _messageService = messageService;

            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
            TaskScheduler.UnobservedTaskException += OnUnobservedTaskException;
            Application.Current.DispatcherUnhandledException += OnDispatcherUnhandledException;
        }

        private void OnUnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            try
            {
                var exception = e.Exception;

                Logger.LogError(exception, null);

                CreateSupportPackage(exception);
            }
            finally
            {
                ExitApplication();
            }
        }

        private void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            try
            {
                var exception = (Exception)e.ExceptionObject;

                Logger.LogError(exception, null);

                CreateSupportPackage(exception);
            }
            finally
            {
                ExitApplication();
            }
        }

        private static void ExitApplication()
        {
            var mainWindow = Application.Current.MainWindow;

            if (mainWindow is not null)
            {
                mainWindow.Close();
            }

            var currentProcess = Process.GetCurrentProcess();
            currentProcess.Kill();
        }

        private void OnDispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            var exception = e.Exception;
            try
            {
                Logger.LogError(exception, null);

                CreateSupportPackage(exception);
                e.Handled = true;
            }
            finally
            {
                ExitApplication();
            }
        }

        private void CreateSupportPackage(Exception exception)
        {
            try
            {
                var message = string.Format("{0}\n\n The application will be terminated.", exception.Message);
                _messageService.ShowAsync(message, "Fatal Error", MessageButton.YesNo, MessageImage.Error).Wait();                
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Failed to create support package.");
            }
        }
    }
}
