// --------------------------------------------------------------------------------------------------------------------
// <copyright file="App.xaml.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2014 Wild Gums. All rights reserved.
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
        protected override async void OnStartup(StartupEventArgs e)
        {
#if DEBUG
            LogManager.AddDebugListener(true);
#endif

            await SquirrelHelper.HandleSquirrelAutomatically();

            var serviceLocator = ServiceLocator.Default;
            var shellService = serviceLocator.ResolveType<IShellService>();
            await shellService.CreateWithSplash<ShellWindow>();

            var googleAnalyticsService = serviceLocator.ResolveType<IGoogleAnalyticsService>();

            _end = DateTime.Now;
            _stopwatch.Stop();

#pragma warning disable 4014
            googleAnalyticsService.SendTiming(_stopwatch.Elapsed, Analytics.Application.Name, Analytics.Application.StartupTime);
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