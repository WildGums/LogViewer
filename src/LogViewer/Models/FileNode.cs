namespace LogViewer.Models
{
    using System;
    using System.Collections.ObjectModel;
    using System.IO;
    using Catel.Collections;

    public class FileNode : NavigationNode
    {
        public FileNode(FileInfo fileInfo)
        {
            ArgumentNullException.ThrowIfNull(fileInfo);

            FileInfo = fileInfo;
            Name = fileInfo.Name;
            FullName = fileInfo.FullName;

            Records = new ObservableCollection<LogRecord>();
        }

        public FileInfo FileInfo { get; set; }
        public bool IsUnifyNamed { get; set; }
        public DateTime DateTime { get; set; }
        public ObservableCollection<LogRecord> Records { get; private set; }
        public bool? IsExpanded { get; set; }

        public override bool AllowMultiSelection
        {
            get { return true; }
        }

        private void OnFileInfoChanged()
        {
            Name = FileInfo.Name;
            FullName = FileInfo.FullName;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
