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
    using Extensions;
    using Models.Base;

    public class MultipleSelectionBehavior : BehaviorBase<TreeView>
    {
        #region Properties
        private NavigationNode StartItem { get; set; }
        #endregion

        #region Methods
        protected override void OnAssociatedObjectLoaded()
        {
            AssociatedObject.SelectedItemChanged += OnTreeViewSelectedItemChanged;
            if (SelectedItems != null)
            {
                SelectedItems.CollectionChanged += OnSelectedItemsCollectionChanged;
            }
        }

        protected override void OnAssociatedObjectUnloaded()
        {
            AssociatedObject.SelectedItemChanged -= OnTreeViewSelectedItemChanged;
            if (SelectedItems != null)
            {
                SelectedItems.CollectionChanged -= OnSelectedItemsCollectionChanged;
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

            await DeSelectAllItems();
            SelectedItems.Add(node);
            StartItem = node;
        }

        private async Task DeSelectAllItems()
        {
            foreach (var node in AssociatedObject.EnumerateNested<TreeViewItem>().Select(x => x.DataContext).OfType<NavigationNode>())
            {
                SelectedItems.Remove(node);
            }
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

            if (SelectedItems.Contains(node))
            {
                SelectedItems.Remove(node);
            }
            else
            {
                SelectedItems.Add(node);
            }

            if (StartItem == null && SelectedItems.Contains(node))
            {
                StartItem = node;
            }

            if (StartItem != null && SelectedItems.Count == 0)
            {
                StartItem = null;
            }

            SelectedItems.RemoveByPredicate(x => !x.AllowMultiselection);
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

                await DeSelectAllItems();
                bool isBetween = false;
                foreach (var item in allItems)
                {
                    if (item == node || item == startItem)
                    {
                        isBetween = !isBetween;

                        SelectedItems.Add(item);
                        continue;
                    }

                    if (isBetween)
                    {
                        SelectedItems.Add(item);
                    }
                }

                SelectedItems.RemoveByPredicate(x => !x.AllowMultiselection);
            }
        }
        #endregion

        #region SelectedItems

        #region Constants
        public static readonly DependencyProperty SelectedItemsProperty = DependencyProperty.RegisterAttached("SelectedItems", typeof (ObservableCollection<NavigationNode>), typeof (MultipleSelectionBehavior), new PropertyMetadata(new ObservableCollection<NavigationNode>(), SelectedItemsChanged));
        #endregion

        #region Properties
        public ObservableCollection<NavigationNode> SelectedItems
        {
            get { return (ObservableCollection<NavigationNode>) GetValue(SelectedItemsProperty); }
            set { SetValue(SelectedItemsProperty, value); }
        }
        #endregion

        #region Methods
        public static ObservableCollection<NavigationNode> GetSelectedItems(MultipleSelectionBehavior element)
        {
            return (ObservableCollection<NavigationNode>) element.GetValue(SelectedItemsProperty);
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
                oldValue.CollectionChanged -= OnSelectedItemsCollectionChanged;
            }
            if (newValue != null)
            {
                newValue.CollectionChanged += OnSelectedItemsCollectionChanged;
            }
        }
        #endregion

        #endregion

        #region IsItemSelected

        #region Constants
        public static readonly DependencyProperty IsItemSelectedProperty = DependencyProperty.RegisterAttached("IsItemSelected", typeof (bool), typeof (MultipleSelectionBehavior), new PropertyMetadata());
        #endregion

        #region Methods
        public static bool GetIsItemSelected(TreeViewItem element)
        {
            return (bool) element.GetValue(IsItemSelectedProperty);
        }

        public static void SetIsItemSelected(TreeViewItem element, bool value)
        {
            element.SetValue(IsItemSelectedProperty, value);
        }
        #endregion

        #endregion
    }
}