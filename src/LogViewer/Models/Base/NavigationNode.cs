// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NavigationNode.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace LogViewer.Models.Base
{
    using System.Collections.ObjectModel;
    using Catel.Data;
    using YAXLib;

    
    public abstract class NavigationNode : ModelBase
    {
        public NavigationNode()
        {
            Children = new ObservableCollection<NavigationNode>();
        }

        [YAXSerializableField]
        public virtual string Name { get; set; }

        [YAXSerializableField]
        public ObservableCollection<NavigationNode> Children { get; set; }
    }
}