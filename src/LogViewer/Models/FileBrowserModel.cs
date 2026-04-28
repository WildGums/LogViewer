namespace LogViewer.Models
{
    using Catel.Collections;
    using Catel.Data;
    using Catel.Services;

    public class FileBrowserModel : ModelBase
    {
        public FileBrowserModel(IDispatcherService dispatcherService)
        {
            RootDirectories = new FastObservableCollection<FolderNode>(dispatcherService);
            SelectedItems = new FastObservableCollection<NavigationNode>(dispatcherService);
        }

        public FastObservableCollection<FolderNode> RootDirectories { get; private set; }
        public FastObservableCollection<NavigationNode> SelectedItems { get; private set; }
    }
}
