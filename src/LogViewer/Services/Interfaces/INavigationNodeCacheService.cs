namespace LogViewer.Services
{
    using Models;

    public interface INavigationNodeCacheService
    {
        T GetFromCache<T>(string fullPath) where T : NavigationNode;
        void AddToCache(NavigationNode folderNode);
        void RemoveFromCache(string fullPath);
    }
}