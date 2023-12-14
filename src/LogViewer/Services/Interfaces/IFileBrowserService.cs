namespace LogViewer.Services
{
    using Models;

    public interface IFileBrowserService
    {
        #region Properties
        FileBrowserModel FileBrowserModel { get; }
        #endregion
    }
}