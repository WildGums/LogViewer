namespace LogViewer.Services
{
    using Catel.Services;
    using Models;

    public class FileBrowserService : IFileBrowserService
    {
        public FileBrowserService(IDispatcherService dispatcherService)
        {
            FileBrowserModel = new FileBrowserModel(dispatcherService);
        }

        public FileBrowserModel FileBrowserModel { get; private set; }
    }
}
