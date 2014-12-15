// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogRecordsGridViewModel.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace LogViewer.ViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    using Catel;
    using Catel.Collections;
    using Catel.MVVM;

    using LogViewer.Models;
    using LogViewer.Models.Base;

    public class LogRecordsGridViewModel : ViewModelBase
    {
        public LogRecordsGridViewModel(LogViewerModel logViewerModel)
        {
            LogViewer = logViewerModel;
            LogRecords = new ObservableCollection<LogRecord>();
        }

        [Model]
        public LogViewerModel LogViewer { get; set; }

        [ViewModelToModel("LogViewer")]
        public TreeNode SelectedItem { get; set; }

        public ObservableCollection<LogRecord> LogRecords { get; set; }

        public void OnSelectedItemChanged()
        {
            LogRecords.Clear();

            LogRecords.AddRange(GetLastChildNodes(SelectedItem).SelectMany(GetLogRecords));
        }

        private IEnumerable<TreeNode> GetLastChildNodes(TreeNode node)
        {
            if (node.Children == null || node.Children.Count == 0)
            {
                yield return node;
                yield break;
            }

            foreach (var last in node.Children.SelectMany(GetLastChildNodes))
            {
                yield return last;
            }
        }

        private IEnumerable<LogRecord> GetLogRecords(TreeNode node)
        {
            Argument.IsNotNull(() => node);            

            var logFile = node as LogFile;
            if (logFile != null)
            {
                return logFile.LogRecords;
            }

            var logFileGroup = node as LogFilesGroup;
            if (logFileGroup != null)
            {
                return logFileGroup.LogFiles.SelectMany(x => x.LogRecords);
            }

            return Enumerable.Empty<LogRecord>();
        }
    }
}