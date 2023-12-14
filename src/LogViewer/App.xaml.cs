namespace LogViewer
{
    using System;
    using System.Diagnostics;
    using System.Windows;
    using Catel.IoC;
    using Catel.Logging;
    using Orc.Squirrel;
    using Orchestra.Services;
    using Orchestra.Views;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private readonly DateTime _start;
        private readonly Stopwatch _stopwatch;
        private DateTime _end;

        public App()
        {
            _stopwatch = new Stopwatch();
            _stopwatch.Start();
            _start = DateTime.Now;
        }

#pragma warning disable AvoidAsyncVoid // Avoid async void
        protected override async void OnStartup(StartupEventArgs e)
#pragma warning restore AvoidAsyncVoid // Avoid async void
        {
#if DEBUG
            LogManager.AddDebugListener(true);
#endif

            await SquirrelHelper.HandleSquirrelAutomaticallyAsync();

            var serviceLocator = ServiceLocator.Default;
            var shellService = serviceLocator.ResolveType<IShellService>();
            await shellService.CreateAsync<ShellWindow>();

            _end = DateTime.Now;
            _stopwatch.Stop();

            Log.Info("Elapsed startup stopwatch time: {0}", _stopwatch.Elapsed);
            Log.Info("Elapsed startup time: {0}", _end - _start);
        }
    }
}
