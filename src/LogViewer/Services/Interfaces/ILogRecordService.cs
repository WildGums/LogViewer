// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ILogRecordService.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace LogViewer.Services
{
    using System.Collections.Generic;

    using LogViewer.Models;

    public interface ILogRecordService
    {
        #region Methods
        IEnumerable<LogRecord> LoadRecordsFromFile(LogFile logFile);
        #endregion
    }
}