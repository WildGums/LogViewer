// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogTabViewModel.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace LogViewer.ViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Catel.Collections;
    using Catel.MVVM;
    using Models;
    using Models.Base;

    public class LogTabViewModel : ViewModelBase
    {
        public LogTabViewModel(LogViewerModel logViewerModel)
        {
            LogViewer = logViewerModel;
            LogTabsData = new ObservableCollection<TreeNode>();
        }

        [Model]
        public LogViewerModel LogViewer { get; set; }

        [ViewModelToModel("LogViewer")]
        public TreeNode SelectedItem { get; set; }

        public ObservableCollection<TreeNode> LogTabsData { get; set; }

        public void OnSelectedItemChanged()
        {
            LogTabsData.Clear();
            LogTabsData.AddRange(GetLastChildNodes(SelectedItem));
        }

        private IEnumerable<TreeNode> GetLastChildNodes(TreeNode node)
        {
            if (node.Children == null || node.Children.Count == 0)
            {
                yield return node;
                yield break;
            }

            foreach (var last in node.Children.SelectMany(child => GetLastChildNodes(child)))
            {
                yield return last;
            }
        }
    }
}