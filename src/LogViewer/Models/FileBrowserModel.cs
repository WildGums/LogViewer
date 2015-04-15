// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileBrowserModel.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2014 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace LogViewer.Models
{
    using System.Collections.ObjectModel;
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