// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IFileBrowserService.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace LogViewer.Services
{
    using Models;

    public interface IFileBrowserService
    {
        #region Properties
        FileBrowserModel FileBrowserModel { get; }
        #endregion
    }
}