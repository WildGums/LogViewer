// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileNode.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2014 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace LogViewer.Models
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using Catel;
    using Lucene.Net.Analysis;
    using Lucene.Net.Analysis.Standard;
    using Lucene.Net.Documents;
    using Lucene.Net.Index;
    using Lucene.Net.QueryParsers;
    using Lucene.Net.Search;
    using Lucene.Net.Store;
    using Version = Lucene.Net.Util.Version;

    public class FileNode : NavigationNode, IDisposable
    {
        public FileNode(FileInfo fileInfo)
        {
            Argument.IsNotNull(() => fileInfo);

            FileInfo = fileInfo;
            Name = fileInfo.Name;
            FullName = fileInfo.FullName;

            LogRecords = new ObservableCollection<LogRecord>();
        }

        public FileInfo FileInfo { get; set; }

        private void OnFileInfoChanged()
        {
            Name = FileInfo.Name;
            FullName = FileInfo.FullName;
        }

        #region Properties
        public FileInfo Info { get; set; }

        public bool IsUnifyNamed { get; set; }

        public DateTime DateTime { get; set; }

        public ObservableCollection<LogRecord> LogRecords { get; private set; }

        public bool? IsExpanded { get; set; }

        public override bool AllowMultiSelection
        {
            get { return true; }
        }

        public IndexSearcher Searcher { get; private set; }

        public void EnsureFullTextIndex()
        {
            string directoryName = this.GetIndexDirectory();
            var directory = FSDirectory.Open(directoryName);

            if (!IndexReader.IndexExists(directory))
            {
                var analyzer = new StandardAnalyzer(Version.LUCENE_30);
                using (var writer = new IndexWriter(directory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
                {
                    foreach (var logRecord in LogRecords)
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

            this.Searcher = new IndexSearcher(directory);
        }

        public IEnumerable<Tuple<LogRecord, float>> Select(string text, Func<LogRecord, bool> where = null)
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
            return this.Select(query, where);
        }

        public IEnumerable<Tuple<LogRecord, float>> Select(Query query, Func<LogRecord, bool> where = null)
        {
            TopDocs search = this.Searcher.Search(query, LogRecords.Count);
            var result = new List<Tuple<LogRecord, float>>();
            foreach (var scoreDoc in search.ScoreDocs)
            {
                float score = scoreDoc.Score;
                int docId = scoreDoc.Doc;
                var doc = this.Searcher.Doc(docId);

                var n = int.Parse(doc.Get("id"));
                if (where == null || where(LogRecords[n]))
                {
                    result.Add(new Tuple<LogRecord, float>(LogRecords[n], score));
                }
            }

            return result;
        }
        #endregion

        private string GetIndexDirectory()
        {
            var lastNdx = this.Info.Name.LastIndexOf(this.Info.Extension, StringComparison.Ordinal);
            var cleanName = this.Info.Name;
            if (lastNdx > 0)
            {
                cleanName = cleanName.Substring(0, lastNdx);
            }
            return this.Info.DirectoryName + @"\Indexes\" + cleanName;
        }

        public void Dispose()
        {
            if (this.Searcher != null)
            {
                this.Searcher.Dispose();
            }
        }        
    }
}