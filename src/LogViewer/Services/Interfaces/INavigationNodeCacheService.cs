// --------------------------------------------------------------------------------------------------------------------
// <copyright file="INavigationNodeCacheService.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
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