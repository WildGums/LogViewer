namespace LogViewer.Services
{
    using Models;

    public interface IFileSystemService
    {
        FolderNode LoadFileSystemContent(string path, bool isNavigationRoot = false);
        void ReleaseFileSystemContent(FolderNode folder);

        string Filter { get; set; }
    }
}