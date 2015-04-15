// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IFilterService.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace LogViewer.Services
{
    using System;
    using Models;

    public interface IFilterService
    {
        #region Properties
        Filter Filter { get; set; }
        #endregion

        #region Methods
        void ApplyFilesFilter();
        void ApplyLogRecordsFilter(FileNode fileNode = null);
        #endregion
    }
}