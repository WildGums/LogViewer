namespace LogViewer.Services
{
    using System.Collections.Generic;
    using Models;

    public class NavigationNodeCacheService : INavigationNodeCacheService
    {
        private readonly IDictionary<string, NavigationNode> _navigationNodes = new Dictionary<string, NavigationNode>();

        public T GetFromCache<T>(string fullPath) 
            where T : NavigationNode
        {
            _navigationNodes.TryGetValue(fullPath, out var result);

            return (T)result;
        }

        public void AddToCache(NavigationNode folderNode)
        {
            _navigationNodes[folderNode.FullName] = folderNode;
        }

        public void RemoveFromCache(string fullPath)
        {
            if (!_navigationNodes.ContainsKey(fullPath))
            {
                return;
            }

            _navigationNodes.Remove(fullPath);
        }
    }
}
