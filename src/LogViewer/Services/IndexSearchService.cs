// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IndexSearchService.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace LogViewer.Services
{
    using System;
    using System.Collections.Generic;
    using Catel;
    using Catel.Logging;
    using Catel.Services;
    using Lucene.Net.Analysis.Standard;
    using Lucene.Net.Documents;
    using Lucene.Net.Index;
    using Lucene.Net.QueryParsers;
    using Lucene.Net.Search;
    using Lucene.Net.Store;
    using Models;
    using Version = Lucene.Net.Util.Version;
    using System.IO;
    using System.Linq;
    using Catel.Collections;
    using MethodTimer;

    // TODO: Replace by Orc.Search
    public class IndexSearchService : IIndexSearchService
    {
        #region Fields
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private readonly IDictionary<string, IndexSearcher> _searchers = new Dictionary<string, IndexSearcher>();
        #endregion

        #region Methods
        [Time]
        public void EnsureFullTextIndex(FileNode file)
        {
            Argument.IsNotNull(() => file);

            Log.Debug("Ensuring full text index for file node '{0}'", file);

            string directoryName = GetIndexDirectory(file);
            var directory = FSDirectory.Open(directoryName);

            if (!IndexReader.IndexExists(directory))
            {
                var analyzer = new StandardAnalyzer(Version.LUCENE_30);
                using (var writer = new IndexWriter(directory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
                {
                    foreach (var logRecord in file.Records)
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

            var indexSearcher = new IndexSearcher(directory);
            _searchers[file.FullName] = indexSearcher;
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
            return Select(file, query, where);
        }

        public IEnumerable<Tuple<LogRecord, float>> Select(FileNode file, Query query, Func<LogRecord, bool> where = null)
        {
            var searcher = _searchers[file.FullName];

            var records = file.Records.ToArray();

            var search = searcher.Search(query, records.Count());
            var result = new List<Tuple<LogRecord, float>>();

            foreach (var scoreDoc in search.ScoreDocs)
            {
                float score = scoreDoc.Score;
                int docId = scoreDoc.Doc;
                var doc = searcher.Doc(docId);

                var n = int.Parse(doc.Get("id"));
                if (where == null || where(records[n]))
                {
                    result.Add(new Tuple<LogRecord, float>(records[n], score));
                }
            }

            return result;
        }

        public void DisposeIndex(FileNode file)
        {
            var key = file.FullName;
            if (_searchers.ContainsKey(key))
            {
                _searchers[key].Dispose();
                _searchers.Remove(key);
            }
        }

        private string GetIndexDirectory(FileNode file)
        {
            var lastIndex = file.FileInfo.Name.LastIndexOf(file.FileInfo.Extension, StringComparison.Ordinal);
            var cleanName = file.FileInfo.Name;
            if (lastIndex > 0)
            {
                cleanName = cleanName.Substring(0, lastIndex);
            }

            return Path.Combine(file.FileInfo.DirectoryName, "Indexes", cleanName);
        }
        #endregion
    }
}