// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IFilterService.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace LogViewer.Services
{
    using LogViewer.Models;

    public interface IFilterService
    {
        #region Methods
        void ApplyFilesFilter(LogViewerModel logViewer);

        void ApplyLogRecodsFilter(LogViewerModel logViewer);
        #endregion
    }
}