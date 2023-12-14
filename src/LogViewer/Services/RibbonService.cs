namespace LogViewer.Services
{
    using System.Windows;

    using LogViewer.Views;

    using Orchestra.Services;

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