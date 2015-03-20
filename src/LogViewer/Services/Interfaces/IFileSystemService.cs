namespace LogViewer.Services
{
    using Models;

    public interface IFileSystemService
    {
        FolderNode LoadFileSystemContent(string path, bool isNavigationRoot = false);
    }
}