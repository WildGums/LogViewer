// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogFilesGroup.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace LogViewer.Models
{
    using System.Collections.ObjectModel;
    using Base;
    using Catel.Collections;
    using Catel.Data;

    public class LogFilesGroup : TreeNode
    {
        public LogFilesGroup()
        {
            LogFiles = new FastObservableCollection<LogFile>();
        }
        public ObservableCollection<LogFile> LogFiles { get; set; }
    }
}