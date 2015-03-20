// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IndexSearchService.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace LogViewer.Services
{
    using System;
    using System.Collections.Generic;
    using Catel.Services;
    using Lucene.Net.Analysis.Standard;
    using Lucene.Net.Documents;
    using Lucene.Net.Index;
    using Lucene.Net.QueryParsers;
    using Lucene.Net.Search;
    using Lucene.Net.Store;
    using Models;
    using Version = Lucene.Net.Util.Version;

    public class IndexSearchService : IIndexSearchService
    {
        private readonly IDispatcherService _dispatcherService;

        #region Fields
        private readonly IDictionary<FileNode, IndexSearcher> _searchers = new Dictionary<FileNode, IndexSearcher>();
        #endregion

        public IndexSearchService(IDispatcherService dispatcherService)
        {
            _dispatcherService = dispatcherService;
        }

        #region Methods
        public void EnsureFullTextIndex(FileNode file)
        {
            string directoryName = this.GetIndexDirectory(file);
            var directory = FSDirectory.Open(directoryName);

            if (!IndexReader.IndexExists(directory))
            {
                var analyzer = new StandardAnalyzer(Version.LUCENE_30);
                using (var writer = new IndexWriter(directory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
                {
                    foreach (var logRecord in file.LogRecords)
                    {
                        var doc = new Document();
                        doc.Add(new Field("id", logRecord.Position.ToString(), Field.Store.YES, Field.Index.NO));
                        doc.Add(new Field("message", logRecord.Message, Field.Store.YES, Field.Index.ANALYZED));
                        writer.AddDocument(doc);
                    }

                    writer.Optimize();
                    writer.Commit();
                }
            }

            _searchers[file] = new IndexSearcher(directory);
        }

        public IEnumerable<Tuple<LogRecord, float>> Select(FileNode file, string text, Func<LogRecord, bool> where = null)
        {
            var analyzer = new StandardAnalyzer(Version.LUCENE_30);
            var parser = new QueryParser(Version.LUCENE_30, "message", analyzer);
            if (!text.Contains("*") &&
                !text.Contains(":") &&
                !text.Contains(" ") &&
                !text.Contains("AND") &&
                !text.Contains("OR"))
            {
                text += "*";
            }
            var query = parser.Parse(text);
            return this.Select(file, query, where);
        }

        public IEnumerable<Tuple<LogRecord, float>> Select(FileNode file, Query query, Func<LogRecord, bool> where = null)
        {
            var searcher = _searchers[file];

            TopDocs search = searcher.Search(query, file.LogRecords.Count);
            var result = new List<Tuple<LogRecord, float>>();
            foreach (var scoreDoc in search.ScoreDocs)
            {
                float score = scoreDoc.Score;
                int docId = scoreDoc.Doc;
                var doc = searcher.Doc(docId);

                var n = int.Parse(doc.Get("id"));
                if (where == null || where(file.LogRecords[n]))
                {
                    result.Add(new Tuple<LogRecord, float>(file.LogRecords[n], score));
                }
            }

            return result;
        }

        public void DisposeIndex(FileNode file)
        {
            if (_searchers.ContainsKey(file))
            {
                _searchers[file].Dispose();
                _searchers.Remove(file);
            }
        }

        private string GetIndexDirectory(FileNode file)
        {
            var lastNdx = file.FileInfo.Name.LastIndexOf(file.FileInfo.Extension, StringComparison.Ordinal);
            var cleanName = file.FileInfo.Name;
            if (lastNdx > 0)
            {
                cleanName = cleanName.Substring(0, lastNdx);
            }
            return file.FileInfo.DirectoryName + @"\Indexes\" + cleanName;
        }
        #endregion
    }
}