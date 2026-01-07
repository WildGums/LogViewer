namespace LogViewer.Models
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Windows.Data;

    public class FolderNode : NavigationNode
    {
        public FolderNode(DirectoryInfo directoryInfo)
        {
            ArgumentNullException.ThrowIfNull(directoryInfo);

            Name = directoryInfo.Name;

            FullName = directoryInfo.FullName;

            Files = new ObservableCollection<FileNode>();
            Directories = new ObservableCollection<FolderNode>();
        }

        public override bool AllowMultiSelection
        {
            get { return false; }
        }

        public IList<FileNode> Files { get; set; }
        public IList<FolderNode> Directories { get; set; }

        public IList Children
        {
            get
            {
                return new CompositeCollection
                {
                    new CollectionContainer
                    {
                        Collection = Directories
                    },
                    new CollectionContainer
                    {
                        Collection = Files
                    }
                };
            }
        }
    }
}
