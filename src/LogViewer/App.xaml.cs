using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using Catel.IoC;
using Orchestra.Markup;
using Orchestra.Services;
using Orchestra.Views;

namespace LogViewer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            FontImage.RegisterFont("FontAwesome", new FontFamily(new Uri("pack://application:,,,/LogViewer;component/Resources/Fonts/", UriKind.RelativeOrAbsolute), "./#FontAwesome"));

            FontImage.DefaultFontFamily = "FontAwesome";

            var serviceLocator = ServiceLocator.Default;
            var shellService = serviceLocator.ResolveType<IShellService>();
            shellService.CreateWithSplash<ShellWindow>();
        }
    }
}
