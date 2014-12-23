// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NavigationNode.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace LogViewer.Models
{
    using System.Collections.ObjectModel;

    using Catel.Data;

    public abstract class NavigationNode : ModelBase
    {
        #region Constructors
        protected NavigationNode()
        {
            Children = new ObservableCollection<NavigationNode>();
        }
        #endregion

        #region Properties
        public string Name { get; set; }

        public bool IsSelected { get; set; }

        public bool IsItemSelected { get; set; }

        public abstract bool AllowMultiSelection { get; }

        public ObservableCollection<NavigationNode> Children { get; private set; }
        #endregion
    }
}