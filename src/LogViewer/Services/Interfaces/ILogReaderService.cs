// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ILogReaderService.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


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