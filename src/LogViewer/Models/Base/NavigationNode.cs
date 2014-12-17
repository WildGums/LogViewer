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
        public NavigationNode()
        {
            Children = new ObservableCollection<NavigationNode>();
        }

        public virtual string Name { get; set; }

        public ObservableCollection<NavigationNode> Children { get; set; }
    }
}