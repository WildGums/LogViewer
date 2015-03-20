// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileNode.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace LogViewer.Models
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using Catel;
    using Lucene.Net.Analysis.Standard;
    using Lucene.Net.Documents;
    using Lucene.Net.Index;
    using Lucene.Net.QueryParsers;
    using Lucene.Net.Search;
    using Lucene.Net.Store;
    using Version = Lucene.Net.Util.Version;

    public class FileNode : NavigationNode//, IDisposable
    {
        #region Constructors
        public FileNode(FileInfo fileInfo)
        {
            Argument.IsNotNull(() => fileInfo);

            FileInfo = fileInfo;
            Name = fileInfo.Name;
            FullName = fileInfo.FullName;

            LogRecords = new ObservableCollection<LogRecord>();
        }
        #endregion

        #region Properties
        public FileInfo FileInfo { get; set; }
        public bool IsUnifyNamed { get; set; }
        public DateTime DateTime { get; set; }
        public ObservableCollection<LogRecord> LogRecords { get; private set; }
        public bool? IsExpanded { get; set; }

        public override bool AllowMultiSelection
        {
            get { return true; }
        }

        #endregion

        #region Methods
        private void OnFileInfoChanged()
        {
            Name = FileInfo.Name;
            FullName = FileInfo.FullName;
        }        
        #endregion
    }
}