namespace LogViewer.Services
{
    using Models;

    public interface IFileSystemService
    {
        #region Properties
        string Filter { get; set; }
        #endregion

        #region Methods
        FolderNode LoadFileSystemContent(string path, bool isNavigationRoot = false);
        void ReleaseFileSystemContent(FolderNode folder);
        #endregion
    }
}