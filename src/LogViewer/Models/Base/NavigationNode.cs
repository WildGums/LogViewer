// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NavigationNode.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace LogViewer.Models.Base
{
    using System.Collections.ObjectModel;

    using Catel.Data;

    public abstract class NavigationNode : ModelBase
    {
        #region Constructors
        public NavigationNode()
        {
            Children = new ObservableCollection<NavigationNode>();
        }
        #endregion

        #region Properties
        public string Name { get; set; }

        public bool IsSelected { get; set; }

        public bool IsItemSelected { get; set; }

        public abstract bool AllowMultiselection { get; }

        public ObservableCollection<NavigationNode> Children { get; set; }
        #endregion
    }
}