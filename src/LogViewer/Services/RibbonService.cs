using System.Windows;
using LogViewer.Views;
using Orchestra.Shell.Services;

namespace LogViewer.Services
{
    public class RibbonService : IRibbonService
    {
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
    }
}