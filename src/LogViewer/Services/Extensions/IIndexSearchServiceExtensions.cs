// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IIndexSearchServiceExtensions.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace LogViewer.Services
{
    using System.Threading.Tasks;
    using Models;

    public static class IIndexSearchServiceExtensions
    {
        public static async Task EnsureFullTextIndexAsync(this IIndexSearchService indexSearchService, FileNode file)
        {
            await Task.Factory.StartNew(() => indexSearchService.EnsureFullTextIndex(file));
        }
    }
}