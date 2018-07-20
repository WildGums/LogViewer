// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NavigationNodeCacheService.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace LogViewer.Services
{
    using System.Collections.Generic;
    using Models;

    public class NavigationNodeCacheService : INavigationNodeCacheService
    {
        #region Fields
        private readonly IDictionary<string, NavigationNode> _navigationNodes = new Dictionary<string, NavigationNode>();
        #endregion

        #region Methods
        public T GetFromCache<T>(string fullPath) where T : NavigationNode
        {
            NavigationNode result;
            _navigationNodes.TryGetValue(fullPath, out result);

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
        #endregion
    }
}