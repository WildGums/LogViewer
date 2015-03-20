// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ILogRecordService.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2014 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace LogViewer.Services
{
    using System.Collections.Generic;

    using LogViewer.Models;

    public interface ILogRecordService
    {
        #region Methods
        IEnumerable<LogRecord> LoadRecordsFromFile(FileNode fileNode);
        #endregion
    }
}