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
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.Linq;
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
            if (SelectedItems != null)
            {
                SelectedItems.CollectionChanged += collection_CollectionChanged;
            }
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
            if (!Equals(e.OriginalSource, AssociatedObject))
            {
                return;
            }

            var treeViewItem = AssociatedObject.FindNodeByDataContext(node);
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
                SelectMultipleItemsRandomly(treeViewItem);
            }
            else if (Keyboard.Modifiers == ModifierKeys.Shift)
            {
                SelectMultipleItemsContinuously(treeViewItem);
            }
            else
            {
                SelectSingleItem(treeViewItem);
            }
        }

        #region Attached Properties

        #region IsItemSelected
        public static readonly DependencyProperty IsItemSelectedProperty = DependencyProperty.RegisterAttached("IsItemSelected", typeof(bool), typeof(MultipleSelectionBehavior), new PropertyMetadata());
 
        public static bool GetIsItemSelected(TreeViewItem element)
        {
            return (bool)element.GetValue(IsItemSelectedProperty);
        }

        public static void SetIsItemSelected(TreeViewItem element, bool value)
        {
            element.SetValue(IsItemSelectedProperty, value);
        }
        #endregion

        #endregion // Attached Properties               

        private void SelectSingleItem(TreeViewItem treeViewItem)
        {
            // first deselect all items
            DeSelectAllItems();
            SelectedItems.Add((NavigationNode)treeViewItem.DataContext);
            StartItem = treeViewItem;
        }

        private void DeSelectAllItems()
        {
            var stack = new Stack<ItemsControl>();
            stack.Push(AssociatedObject);
            while (stack.Count != 0)
            {
                var item = stack.Pop();
                var node = item.DataContext as NavigationNode;
                if (node != null)
                {
                    SelectedItems.Remove(node);
                }
                foreach (var subItem in item.Items.OfType<TreeViewItem>())
                {
                    stack.Push(subItem);
                }
            }            
        }
        
        public static readonly DependencyProperty SelectedItemsProperty = DependencyProperty.RegisterAttached("SelectedItems", typeof(ObservableCollection<NavigationNode>), typeof(MultipleSelectionBehavior), new PropertyMetadata(new ObservableCollection<NavigationNode>(), SelectedItemsChanged));        

        public static ObservableCollection<NavigationNode> GetSelectedItems(MultipleSelectionBehavior element)
        {
            return (ObservableCollection<NavigationNode>)element.GetValue(SelectedItemsProperty);
        }

        public static void SetSelectedItems(MultipleSelectionBehavior element, IList value)
        {
            element.SetValue(SelectedItemsProperty, value);
        }       

        private static void SelectedItemsChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            var newValue = args.NewValue as ObservableCollection<NavigationNode>;
            var oldValue = args.OldValue as ObservableCollection<NavigationNode>;
            if (oldValue != null)
            {
                oldValue.CollectionChanged -= collection_CollectionChanged;
            }
            if (newValue != null)
            {
                newValue.CollectionChanged += collection_CollectionChanged;
            }            
        }

        static void collection_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (var item in e.NewItems.OfType<NavigationNode>())
                    {
                        item.IsItemSelected = true;
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (var item in e.OldItems.OfType<NavigationNode>())
                    {
                        item.IsItemSelected = false;
                    }
                    break;
            }
        }

        public ObservableCollection<NavigationNode> SelectedItems
        {
            get { return (ObservableCollection<NavigationNode>)GetValue(SelectedItemsProperty); }
            set { SetValue(SelectedItemsProperty, value); }
        }

        private TreeViewItem StartItem { get; set; }

        private void SelectMultipleItemsRandomly(TreeViewItem treeViewItem)
        {
            if (SelectedItems.Contains((NavigationNode)treeViewItem.DataContext))
            {
                SelectedItems.Remove((NavigationNode)treeViewItem.DataContext);
            }
            else
            {
                SelectedItems.Add((NavigationNode)treeViewItem.DataContext);
            }

            if (StartItem == null)
            {
                if (SelectedItems.Contains((NavigationNode)treeViewItem.DataContext))
                {
                    StartItem = treeViewItem;
                }
            }
            else
            {
                if (SelectedItems.Count == 0)
                {
                    StartItem = null;
                }
            }
        }

        private void SelectMultipleItemsContinuously(TreeViewItem treeViewItem)
        {
            TreeViewItem startItem = StartItem;
            if (startItem != null)
            {
                if (startItem == treeViewItem)
                {
                    SelectSingleItem(treeViewItem);
                    return;
                }

                ICollection<TreeViewItem> allItems = new List<TreeViewItem>(GetAllItems());
                
                DeSelectAllItems();
                bool isBetween = false;
                foreach (var item in allItems)
                {
                    if (item == treeViewItem || item == startItem)
                    {
                        // toggle to true if first element is found and
                        // back to false if last element is found
                        isBetween = !isBetween;

                        // set boundary element
                        SelectedItems.Add((NavigationNode)item.DataContext);
                        continue;
                    }

                    if (isBetween)
                    {
                        SelectedItems.Add((NavigationNode)item.DataContext);
                    }
                }
            }


        }

        private IEnumerable<TreeViewItem> GetAllItems(TreeViewItem rootItem = null)
        {
            var stack = new Stack<ItemsControl>();
            if (rootItem == null)
            {
                stack.Push(AssociatedObject);
            }
            else
            {
                stack.Push(rootItem);
            }
            while (stack.Count != 0)
            {
                var item = stack.Pop();
                
                if (!Equals(item, AssociatedObject) || Equals(item, rootItem))
                {
                    yield return (TreeViewItem) item;
                }
                foreach (var subItem in item.Items.OfType<TreeViewItem>())
                {
                    stack.Push(subItem);
                }
            }                        
        }
    }
}