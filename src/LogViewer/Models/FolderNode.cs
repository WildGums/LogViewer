// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FolderNode.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace LogViewer.Models
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Windows.Data;
    using Catel;

    public class FolderNode : NavigationNode
    {
        #region Constructors
        public FolderNode(DirectoryInfo directoryInfo)
        {
            Argument.IsNotNull(() => directoryInfo);

            Name = directoryInfo.Name;

            FullName = directoryInfo.FullName;

            Files = new ObservableCollection<FileNode>();
            Directories = new ObservableCollection<FolderNode>();
        }
        #endregion

        #region Properties
        public override bool AllowMultiSelection
        {
            get { return false; }
        }

        public IList<FileNode> Files { get; set; }
        public IList<FolderNode> Directories { get; set; }

        public IList Children
        {
            get { return new CompositeCollection {new CollectionContainer {Collection = Directories}, new CollectionContainer {Collection = Files}}; }
        }
        #endregion
    }
}