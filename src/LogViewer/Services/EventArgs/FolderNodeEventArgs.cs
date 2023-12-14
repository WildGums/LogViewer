﻿namespace LogViewer.Models
{
    using System;
    using System.IO;

    public class FolderNodeEventArgs : EventArgs
    {
        public FolderNodeEventArgs(WatcherChangeTypes changeType, string oldPath, string newPath)
        {
            ChangeType = changeType;
            OldPath = oldPath;
            NewPath = newPath;
        }

        public WatcherChangeTypes ChangeType { get; private set; }
        public string OldPath { get; private set; }
        public string NewPath { get; private set; }
    }
}