namespace LogViewer
{
    using System;
    using System.Diagnostics;
    using System.Threading.Tasks;
    using System.Windows;
    using Catel;
    using Catel.Logging;
    using Catel.Services;

    public class UnhandledExceptionWatcher
    {
        #region Fields
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();
        private readonly IMessageService _messageService;
        #endregion

        #region Constructors
        public UnhandledExceptionWatcher(IMessageService messageService)
        {
            ArgumentNullException.ThrowIfNull(messageService);

            _messageService = messageService;

            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
            TaskScheduler.UnobservedTaskException += OnUnobservedTaskException;
            Application.Current.DispatcherUnhandledException += OnDispatcherUnhandledException;
        }
        #endregion

        #region Methods
        private void OnUnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            try
            {
                var exception = e.Exception;

                Log.Error(exception);

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

                Log.Error(exception);

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
                Log.Error(exception);

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
                Log.Error(ex, "Failed to create support package.");
            }
        }
        #endregion
    }
}