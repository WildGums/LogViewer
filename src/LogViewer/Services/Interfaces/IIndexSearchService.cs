// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IIndexSearchService.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace LogViewer.Services
{
    using System;
    using System.Collections.Generic;
    using Lucene.Net.Search;
    using Models;

    public interface IIndexSearchService
    {
        void EnsureFullTextIndex(FileNode file);
        IEnumerable<Tuple<LogRecord, float>> Select(FileNode file, string text, Func<LogRecord, bool> where = null);
        IEnumerable<Tuple<LogRecord, float>> Select(FileNode file, Query query, Func<LogRecord, bool> where = null);
        void DisposeIndex(FileNode file);
    }
}