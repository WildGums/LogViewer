// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MultipleSelectionBehavior.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace LogViewer.Behaviors
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;
    using Catel.Windows;
    using Catel.Windows.Interactivity;
    using Extensions;
    using Models.Base;
    using ViewModels;
    using Views;

    public class MultipleSelectionBehavior : BehaviorBase<TreeView>
    {
        protected override void OnAssociatedObjectLoaded()
        {
            AssociatedObject.SelectedItemChanged += OnTreeViewSelectedItemChanged;
        }

        protected override void OnAssociatedObjectUnloaded()
        {
            AssociatedObject.SelectedItemChanged -= OnTreeViewSelectedItemChanged;
        }

        private void OnTreeViewSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var node = e.NewValue as NavigationNode;
            if (node == null)
            {
                return;
            }
            var treeView = e.OriginalSource as TreeView;
            if (treeView == null)
            {
                return;
            }

            var treeViewItem = treeView.FindNodeByDataContext(node);
            if (treeViewItem == null)
            {
                return;
            }

            if (node.IsSelected)
            {
                node.IsSelected = false;               
            }

            if (Keyboard.Modifiers == ModifierKeys.Control && node.AllowMultiselection)
            {
                SelectMultipleItemsRandomly(treeView, treeViewItem);
            }
            else if (Keyboard.Modifiers == ModifierKeys.Shift)
            {
                SelectMultipleItemsContinuously(treeView, treeViewItem);
            }
            else
            {
                SelectSingleItem(treeView, treeViewItem);
            }
        }

        #region Attached Properties

        #region IsItemSelected
        public static readonly DependencyProperty IsItemSelectedProperty = DependencyProperty.RegisterAttached("IsItemSelected", typeof(bool), typeof(MultipleSelectionBehavior), new PropertyMetadata(false, OnIsItemSelectedPropertyChanged));

        public static bool GetIsItemSelected(TreeViewItem element)
        {
            return (bool)element.GetValue(IsItemSelectedProperty);
        }

        public static void SetIsItemSelected(TreeViewItem element, bool value)
        {
            element.SetValue(IsItemSelectedProperty, value);
        }
        #endregion

        private static void OnIsItemSelectedPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TreeViewItem treeViewItem = d as TreeViewItem;
            TreeView treeView = FindTreeView(treeViewItem);
            if (treeViewItem != null && treeView != null)
            {
                var selectedItems = GetSelectedItems(treeView);
                if (selectedItems != null)
                {
                    if (GetIsItemSelected(treeViewItem))
                    {
                        selectedItems.Add(treeViewItem.Header);
                    }
                    else
                    {
                        selectedItems.Remove(treeViewItem.Header);
                    }
                }
            }
        }

        #endregion // Attached Properties

        

        
        private static TreeViewItem FindTreeViewItem(DependencyObject dependencyObject)
        {
            if (dependencyObject == null)
            {
                return null;
            }

            TreeViewItem treeViewItem = dependencyObject as TreeViewItem;
            if (treeViewItem != null)
            {
                return treeViewItem;
            }

            return FindTreeViewItem(VisualTreeHelper.GetParent(dependencyObject));
        }

        private static void SelectSingleItem(TreeView treeView,
                                                    TreeViewItem treeViewItem)
        {
            // first deselect all items
            DeSelectAllItems(treeView, null);
            SetIsItemSelected(treeViewItem, true);
            SetStartItem(treeView, treeViewItem);
        }

        private static void DeSelectAllItems(TreeView treeView,
                                                 TreeViewItem treeViewItem)
        {
            if (treeView != null)
            {
                for (int i = 0; i < treeView.Items.Count; i++)
                {
                    TreeViewItem item = treeView.ItemContainerGenerator.
                                               ContainerFromIndex(i) as TreeViewItem;
                    if (item != null)
                    {
                        SetIsItemSelected(item, false);
                        DeSelectAllItems(null, item);
                    }
                }
            }
            else
            {
                for (int i = 0; i < treeViewItem.Items.Count; i++)
                {
                    TreeViewItem item = treeViewItem.ItemContainerGenerator.
                                               ContainerFromIndex(i) as TreeViewItem;
                    if (item != null)
                    {
                        SetIsItemSelected(item, false);
                        DeSelectAllItems(null, item);
                    }
                }
            }
        }

        

        private static TreeView FindTreeView(DependencyObject dependencyObject)
        {
            if (dependencyObject == null)
            {
                return null;
            }

            TreeView treeView = dependencyObject as TreeView;
            if (treeView != null)
            {
                return treeView;
            }

            return FindTreeView(VisualTreeHelper.GetParent(dependencyObject));
        }

        public static readonly DependencyProperty SelectedItemsProperty = DependencyProperty.RegisterAttached("SelectedItems", typeof(IList), typeof(MultipleSelectionBehavior), new PropertyMetadata());

        public static IList GetSelectedItems(TreeView element)
        {
            return (IList)element.GetValue(SelectedItemsProperty);
        }

        public static void SetSelectedItems(TreeView element, IList value)
        {
            element.SetValue(SelectedItemsProperty, value);
        }

        private static readonly DependencyProperty StartItemProperty =
        DependencyProperty.RegisterAttached(
            "StartItem",
            typeof(TreeViewItem),
            typeof(MultipleSelectionBehavior),
            new PropertyMetadata());

        private static TreeViewItem GetStartItem(TreeView element)
        {
            return (TreeViewItem)element.GetValue(StartItemProperty);
        }

        private static void SetStartItem(TreeView element, TreeViewItem value)
        {
            element.SetValue(StartItemProperty, value);
        }

        private static void SelectMultipleItemsRandomly(TreeView treeView,
                                                    TreeViewItem treeViewItem)
        {
            SetIsItemSelected(treeViewItem, !GetIsItemSelected(treeViewItem));
            if (GetStartItem(treeView) == null)
            {
                if (GetIsItemSelected(treeViewItem))
                {
                    SetStartItem(treeView, treeViewItem);
                }
            }
            else
            {
                if (GetSelectedItems(treeView).Count == 0)
                {
                    SetStartItem(treeView, null);
                }
            }
        }

        private static void SelectMultipleItemsContinuously(TreeView treeView,
                                                     TreeViewItem treeViewItem)
        {
            TreeViewItem startItem = GetStartItem(treeView);
            if (startItem != null)
            {
                if (startItem == treeViewItem)
                {
                    SelectSingleItem(treeView, treeViewItem);
                    return;
                }

                ICollection<TreeViewItem> allItems = new List<TreeViewItem>();
                GetAllItems(treeView, null, allItems);
                DeSelectAllItems(treeView, null);
                bool isBetween = false;
                foreach (var item in allItems)
                {
                    if (item == treeViewItem || item == startItem)
                    {
                        // toggle to true if first element is found and
                        // back to false if last element is found
                        isBetween = !isBetween;

                        // set boundary element
                        SetIsItemSelected(item, true);
                        continue;
                    }

                    if (isBetween)
                    {
                        SetIsItemSelected(item, true);
                    }
                }
            }


        }

        private static void GetAllItems(TreeView treeView, TreeViewItem treeViewItem,
                                    ICollection<TreeViewItem> allItems)
        {
            if (treeView != null)
            {
                for (int i = 0; i < treeView.Items.Count; i++)
                {
                    TreeViewItem item = treeView.ItemContainerGenerator.
                                               ContainerFromIndex(i) as TreeViewItem;
                    if (item != null)
                    {
                        allItems.Add(item);
                        GetAllItems(null, item, allItems);
                    }
                }
            }
            else
            {
                for (int i = 0; i < treeViewItem.Items.Count; i++)
                {
                    TreeViewItem item = treeViewItem.ItemContainerGenerator.
                                               ContainerFromIndex(i) as TreeViewItem;
                    if (item != null)
                    {
                        allItems.Add(item);
                        GetAllItems(null, item, allItems);
                    }
                }
            }
        }
    }
}