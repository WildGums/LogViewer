// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileBrowserModel.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2014 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace LogViewer.Models
{
    using System.Collections.ObjectModel;

    using Catel.Data;

    public class FileBrowserModel : ModelBase
    {
        #region Constructors
        public FileBrowserModel()
        {
            RootDirectories = new ObservableCollection<FolderNode>();
            SelectedItems = new ObservableCollection<NavigationNode>();           
        }
        #endregion

        #region Properties
        public ObservableCollection<FolderNode> RootDirectories { get; private set; }

        public ObservableCollection<NavigationNode> SelectedItems { get; private set; }        
        #endregion
    }
}