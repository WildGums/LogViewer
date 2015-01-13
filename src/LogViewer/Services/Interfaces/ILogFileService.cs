// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ILogFileService.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2014 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace LogViewer.Services
{
    using System.Collections.Generic;

    using LogViewer.Models;

    public interface ILogFileService
    {
        #region Methods
        IEnumerable<LogFile> GetLogFiles(string filesFolder);
        #endregion
    }
}