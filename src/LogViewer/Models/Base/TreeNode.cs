// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TreeNode.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace LogViewer.Models.Base
{
    using System.Collections.ObjectModel;
    using Catel.Data;
    using YAXLib;

    
    public abstract class TreeNode : ModelBase
    {
        public TreeNode()
        {
            Children = new ObservableCollection<TreeNode>();
        }

        [YAXSerializableField]
        public virtual string Name { get; set; }

        [YAXSerializableField]
        public ObservableCollection<TreeNode> Children { get; set; }
    }
}