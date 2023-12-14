namespace LogViewer.Services
{
    using System.Threading.Tasks;
    using Models;

    public static class IFileSystemServiceExtensions
    {
        public static async Task<FolderNode> LoadFileSystemContentAsync(this IFileSystemService fileSystemService, string path, bool isNavigationRoot = false)
        {
            return await Task.Factory.StartNew(() => fileSystemService.LoadFileSystemContent(path, isNavigationRoot));
        }
    }
}