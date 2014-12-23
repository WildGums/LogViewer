// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ILogFileService.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
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