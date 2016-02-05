// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileNode.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace LogViewer.Models
{
    using System;
    using System.Collections.ObjectModel;
    using System.IO;
    using Catel;
    using Catel.Collections;

    public class FileNode : NavigationNode
    {
        #region Constructors
        public FileNode(FileInfo fileInfo)
        {
            Argument.IsNotNull(() => fileInfo);

            FileInfo = fileInfo;
            Name = fileInfo.Name;
            FullName = fileInfo.FullName;

            Records = new FastObservableCollection<LogRecord>();
        }
        #endregion

        #region Properties
        public FileInfo FileInfo { get; set; }
        public bool IsUnifyNamed { get; set; }
        public DateTime DateTime { get; set; }
        public FastObservableCollection<LogRecord> Records { get; private set; }
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

        public override string ToString()
        {
            return Name;
        }
        #endregion
    }
}