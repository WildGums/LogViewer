// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TreeViewExtensions.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace LogViewer.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using Catel.Windows;

    public static class TreeViewExtensions
    {
        public static TreeViewItem FindNodeByDataContext(this TreeView treeView, object dataContext)
        {
            var item = treeView.FindNestedVisualDescendantsByType<TreeViewItem>().FirstOrDefault(x => object.Equals(x.DataContext, dataContext));
            return item;
        }
    }
}