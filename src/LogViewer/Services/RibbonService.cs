// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RibbonService.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace LogViewer.Services
{
    using System.Windows;

    using LogViewer.Views;

    using Orchestra.Shell.Services;

    public class RibbonService : IRibbonService
    {
        #region IRibbonService Members
        public FrameworkElement GetRibbon()
        {
            return new RibbonView();
        }

        public FrameworkElement GetMainView()
        {
            return new MainView();
        }

        public FrameworkElement GetStatusBar()
        {
            return new StatusBarView();
        }
        #endregion
    }
}