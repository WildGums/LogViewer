namespace LogViewer.Views
{
    using Catel.Windows;
    using ViewModels;

    public partial class SettingsWindow
    {
        partial void OnInitializedComponent()
        {
            Mode = Catel.Windows.DataWindowMode.OkCancel;
        }
    }
}
