namespace LogViewer.Services
{
    using System.Collections.Generic;

    public interface IFileBrowserConfigurationService
    {
        IEnumerable<string> LoadFolders();
        bool AddFolder(string fullPath);
        void RemoveFolder(string fullPath);
    }
}