// --------------------------------------------------------------------------------------------------------------------
// <copyright file="App.xaml.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace LogViewer
{
    using System;
    using System.Diagnostics;
    using System.Windows;
    using Catel.ApiCop;
    using Catel.ApiCop.Listeners;
    using Catel.IoC;
    using Catel.Logging;
    using Orc.Analytics;
    using Orc.Squirrel;
    using Orchestra.Services;
    using Orchestra.Views;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        #region Constants
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();
        #endregion

        #region Fields
        private readonly DateTime _start;
        private readonly Stopwatch _stopwatch;
        private DateTime _end;
        #endregion

        #region Constructors
        public App()
        {
            _stopwatch = new Stopwatch();
            _stopwatch.Start();
            _start = DateTime.Now;
        }
        #endregion

        #region Methods
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

            var analyticsService = serviceLocator.ResolveType<IAnalyticsService>();

            _end = DateTime.Now;
            _stopwatch.Stop();

#pragma warning disable 4014
            analyticsService.SendTimingAsync(_stopwatch.Elapsed, Analytics.Application.Name, Analytics.Application.StartupTime);
#pragma warning restore 4014

            Log.Info("Elapsed startup stopwatch time: {0}", _stopwatch.Elapsed);
            Log.Info("Elapsed startup time: {0}", _end - _start);
        }

        protected override void OnExit(ExitEventArgs e)
        {
#if DEBUG
            var apiCopListener = new ConsoleApiCopListener();
            ApiCopManager.AddListener(apiCopListener);
            ApiCopManager.WriteResults();
#endif

            base.OnExit(e);
        }
        #endregion
    }
}
