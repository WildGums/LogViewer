// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MultipleSelectionBehavior.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace LogViewer.Behaviors
{
    using System.Collections;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    using Catel;
    using Catel.Windows.Interactivity;

    using LogViewer.Extensions;
    using LogViewer.Models.Base;

    public class MultipleSelectionBehavior : BehaviorBase<TreeView>
    {
        #region Dependency properties
        public static readonly DependencyProperty SelectedItemsProperty = DependencyProperty.RegisterAttached("SelectedItems", typeof(ObservableCollection<NavigationNode>), typeof(MultipleSelectionBehavior), new PropertyMetadata(new ObservableCollection<NavigationNode>(), SelectedItemsChanged));

        public static ObservableCollection<NavigationNode> GetSelectedItems(MultipleSelectionBehavior element)
        {
            return (ObservableCollection<NavigationNode>)element.GetValue(SelectedItemsProperty);
        }

        public static void SetSelectedItems(MultipleSelectionBehavior element, IList value)
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

        public ObservableCollection<NavigationNode> SelectedItems
        {
            get
            {
                return (ObservableCollection<NavigationNode>)GetValue(SelectedItemsProperty);
            }
            set
            {
                SetValue(SelectedItemsProperty, value);
            }
        }

        
        #endregion

        #region Methods
        protected override void OnAssociatedObjectLoaded()
        {
            AssociatedObject.SelectedItemChanged += OnTreeViewSelectedItemChanged;
            var selectedItems = SelectedItems;

            if (selectedItems != null)
            {
                selectedItems.CollectionChanged += OnSelectedItemsCollectionChanged;
            }
        }

        protected override void OnAssociatedObjectUnloaded()
        {
            AssociatedObject.SelectedItemChanged -= OnTreeViewSelectedItemChanged;
            var selectedItems = SelectedItems;

            if (selectedItems != null)
            {
                selectedItems.CollectionChanged -= OnSelectedItemsCollectionChanged;
            }
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

            if (Keyboard.Modifiers == ModifierKeys.Control && node.AllowMultiselection)
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

        private async Task SelectSingleItem(NavigationNode node)
        {
            Argument.IsNotNull(() => node);

            var selectedItems = SelectedItems;

            await selectedItems.ClearOneByOne();
            selectedItems.Add(node);
            StartItem = node;
        }

        private static void OnSelectedItemsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
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

        private async Task SelectMultipleItemsRandomly(NavigationNode node)
        {
            Argument.IsNotNull(() => node);

            var selectedItems = SelectedItems;

            if (selectedItems.Contains(node))
            {
                selectedItems.Remove(node);
            }
            else
            {
                selectedItems.Add(node);
            }

            if (StartItem == null && selectedItems.Contains(node))
            {
                StartItem = node;
            }

            if (StartItem != null && selectedItems.Count == 0)
            {
                StartItem = null;
            }

            selectedItems.RemoveByPredicate(x => !x.AllowMultiselection);
        }

        private async Task SelectMultipleItemsContinuously(NavigationNode node)
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

                var allItems = AssociatedObject.EnumerateNested<TreeViewItem>().Select(x => x.DataContext).OfType<NavigationNode>().ToList().Where(x => x.AllowMultiselection);

                var selectedItems = SelectedItems;

                await selectedItems.ClearOneByOne();

                bool isBetween = false;
                foreach (var item in allItems)
                {
                    if (item == node || item == startItem)
                    {
                        isBetween = !isBetween;

                        selectedItems.Add(item);
                        continue;
                    }

                    if (isBetween)
                    {
                        selectedItems.Add(item);
                    }
                }

                selectedItems.RemoveByPredicate(x => !x.AllowMultiselection);
            }
        }       

        private static void SelectedItemsChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            var newValue = args.NewValue as ObservableCollection<NavigationNode>;
            var oldValue = args.OldValue as ObservableCollection<NavigationNode>;

            if (oldValue != null)
            {
                oldValue.CollectionChanged -= OnSelectedItemsCollectionChanged;
            }
            if (newValue != null)
            {
                newValue.CollectionChanged += OnSelectedItemsCollectionChanged;
            }
        }        
        #endregion
    }
}