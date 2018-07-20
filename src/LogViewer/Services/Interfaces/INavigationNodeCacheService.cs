// --------------------------------------------------------------------------------------------------------------------
// <copyright file="INavigationNodeCacheService.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


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