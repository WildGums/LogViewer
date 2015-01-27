// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogFile.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2014 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace LogViewer.Models
{
    using System;
    using System.Collections.ObjectModel;
    using System.IO;
    using Lucene.Net.Analysis;
    using Lucene.Net.Analysis.Standard;
    using Lucene.Net.Documents;
    using Lucene.Net.Index;
    using Lucene.Net.Store;
    using Version = Lucene.Net.Util.Version;

    public class LogFile : NavigationNode
    {
        public LogFile()
        {
            LogRecords = new ObservableCollection<LogRecord>();
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
            return this.Info.DirectoryName + @"\Indices\" + cleanName;
        }
    }
}