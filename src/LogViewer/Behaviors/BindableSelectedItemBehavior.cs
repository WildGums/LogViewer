// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BindableSelectedItemBehavior.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace LogViewer.Behaviors
{
    using System.Windows.Controls;
    using Catel.Windows;
    using Catel.Windows.Interactivity;
    using Models.Base;
    using ViewModels;
    using Views;

    public class BindableSelectedItemBehavior : BehaviorBase<TreeView>
    {
        protected override void OnAssociatedObjectLoaded()
        {
            AssociatedObject.SelectedItemChanged += OnTreeViewSelectedItemChanged;
        }        

        protected override void OnAssociatedObjectUnloaded()
        {
            AssociatedObject.SelectedItemChanged -= OnTreeViewSelectedItemChanged;
        }

        private IHasSelectableItems ViewModel
        {
            get
            {
                var logTreeView = AssociatedObject.GetAncestorObject<LogTreeView>();
                return logTreeView.ViewModel as LogTreeViewModel;
            }
        }

        void OnTreeViewSelectedItemChanged(object sender, System.Windows.RoutedPropertyChangedEventArgs<object> e)
        {
            if (ViewModel != null)
            {
                ViewModel.SelectedItem = (TreeNode)e.NewValue;
            }
        }
    }
}