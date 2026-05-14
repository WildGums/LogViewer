namespace LogViewer.Views
{
    using Catel.Windows;
    using ViewModels;

    public partial class SettingsWindow
    {
        partial void OnInitializingComponent()
        {
            Mode = Catel.Windows.DataWindowMode.OkCancel;
        }
    }
}
