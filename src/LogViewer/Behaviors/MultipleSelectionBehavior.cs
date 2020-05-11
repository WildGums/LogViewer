// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MultipleSelectionBehavior.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace LogViewer.Behaviors
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    using Catel;
    using Catel.Collections;
    using Catel.Windows.Interactivity;

    using Models;

    public class MultipleSelectionBehavior : BehaviorBase<TreeView>
    {
        #region Dependency properties

        public static FastObservableCollection<NavigationNode> GetSelectedItems(MultipleSelectionBehavior element)
        {
            return (FastObservableCollection<NavigationNode>)element.GetValue(SelectedItemsProperty);
        }

        public static void SetSelectedItems(MultipleSelectionBehavior element, FastObservableCollection<NavigationNode> value)
        {
            element.SetValue(SelectedItemsProperty, value);
        }

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

        #region Properties
        private NavigationNode StartItem { get; set; }

        public FastObservableCollection<NavigationNode> SelectedItems
        {
            get { return (FastObservableCollection<NavigationNode>)GetValue(SelectedItemsProperty); }
            set { SetValue(SelectedItemsProperty, value); }
        }

        public static readonly DependencyProperty SelectedItemsProperty = DependencyProperty.RegisterAttached(nameof(SelectedItems), typeof(FastObservableCollection<NavigationNode>),
#pragma warning disable WPF0016 // Default value is shared reference type.
            typeof(MultipleSelectionBehavior), new PropertyMetadata(new FastObservableCollection<NavigationNode>()));
#pragma warning restore WPF0016 // Default value is shared reference type.
        #endregion

        #region Methods
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
            if (!Equals(e.OriginalSource, AssociatedObject))
            {
                return;
            }

            if (node.IsSelected)
            {
                node.IsSelected = false;
            }

            if (Keyboard.Modifiers == ModifierKeys.Control && node.AllowMultiSelection)
            {
                SelectMultipleItemsRandomly(node);
            }
            else if (Keyboard.Modifiers == ModifierKeys.Shift)
            {
                SelectMultipleItemsContinuously(node);
            }
            else
            {
                SelectSingleItem(node);
            }
        }

        private void SelectSingleItem(NavigationNode node)
        {
            Argument.IsNotNull(() => node);

            var selectedItems = SelectedItems;

            using (selectedItems.SuspendChangeNotifications())
            {
                using (UpdatingSelection(selectedItems))
                {
                    selectedItems.Clear();
                    selectedItems.Add(node);
                }
            }

            StartItem = node;
        }

        private void SelectMultipleItemsRandomly(NavigationNode node)
        {
            Argument.IsNotNull(() => node);

            var selectedItems = SelectedItems;

            var navigationNodes = selectedItems.ToList();

            using (UpdatingSelection(navigationNodes))
            {
                if (navigationNodes.Contains(node))
                {
                    navigationNodes.Remove(node);
                }
                else
                {
                    navigationNodes.Add(node);
                }

                if (StartItem == null && navigationNodes.Contains(node))
                {
                    StartItem = node;
                }

                if (StartItem != null && navigationNodes.Count == 0)
                {
                    StartItem = null;
                }

                navigationNodes.RemoveByPredicate(x => !x.AllowMultiSelection);
            }

            ReplaceRange(selectedItems, navigationNodes);
        }

        private void ReplaceRange(FastObservableCollection<NavigationNode> selectedItems, List<NavigationNode> navigationNodes)
        {
            using (selectedItems.SuspendChangeNotifications())
            {
                ((ICollection<NavigationNode>)SelectedItems).ReplaceRange(navigationNodes);
            }
        }

        private static IDisposable UpdatingSelection(IEnumerable<NavigationNode> selectedItems)
        {
            return new DisposableToken(null, t => SetItemSetectedValue(selectedItems, false), t => SetItemSetectedValue(selectedItems, true));
        }

        private static void SetItemSetectedValue(IEnumerable<NavigationNode> selectedItems, bool value)
        {
            foreach (var selectedItem in selectedItems)
            {
                selectedItem.IsItemSelected = value;
            }
        }

        private void SelectMultipleItemsContinuously(NavigationNode node)
        {
            Argument.IsNotNull(() => node);

            var startItem = StartItem;
            if (startItem != null)
            {
                if (startItem == node)
                {
                    SelectSingleItem(node);
                    return;
                }

                var allItems = AssociatedObject.EnumerateNested<TreeViewItem>().Select(x => x.DataContext).OfType<NavigationNode>().ToList().Where(x => x.AllowMultiSelection);

                var selectedItems = SelectedItems;

                var navigationNodes = new List<NavigationNode>();

                using (UpdatingSelection(navigationNodes))
                {
                    var isBetween = false;
                    foreach (var item in allItems)
                    {
                        if (item == node || item == startItem)
                        {
                            isBetween = !isBetween;

                            navigationNodes.Add(item);
                            continue;
                        }

                        if (isBetween)
                        {
                            navigationNodes.Add(item);
                        }
                    }

                    navigationNodes.RemoveByPredicate(x => !x.AllowMultiSelection);
                }

                ReplaceRange(selectedItems, navigationNodes);
            }
        }            
        #endregion
    }
}
