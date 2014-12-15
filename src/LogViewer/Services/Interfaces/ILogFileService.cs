namespace LogViewer.Services
{
    using System.Collections.Generic;
    using Models;
    using Models.Base;

    public interface ILogFileService
    {
        IEnumerable<LogFile> GetLogFIles(string filesFolder);
    }
}