﻿namespace LogViewer.Models
{
    using Catel.Collections;
    using Catel.Data;

    public class FileBrowserModel : ModelBase
    {
        #region Constructors
        public FileBrowserModel()
        {
            RootDirectories = new FastObservableCollection<FolderNode>();
            SelectedItems = new FastObservableCollection<NavigationNode>();
        }
        #endregion

        #region Properties
        public FastObservableCollection<FolderNode> RootDirectories { get; private set; }
        public FastObservableCollection<NavigationNode> SelectedItems { get; private set; }
        #endregion
    }
}