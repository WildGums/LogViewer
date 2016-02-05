// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IFilterService.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace LogViewer.Services
{
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