// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ILogReaderService.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
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