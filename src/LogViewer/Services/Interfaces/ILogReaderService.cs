namespace LogViewer.Services
{
    using System.Collections.Generic;
    using Models;

    public interface ILogReaderService
    {
        #region Methods
        IEnumerable<LogRecord> LoadRecordsFromFile(FileNode fileNode);
        #endregion
    }
}